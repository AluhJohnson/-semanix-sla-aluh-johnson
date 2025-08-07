using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Semanix.Api;
using Semanix.Application.Config;
using Semanix.Application.EventStore;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Application.Middlewares;
using Semanix.Application.RequestHandler;
using Semanix.Application.Response;
using Semanix.Common.Handlers;
using Semanix.Domain;
using Semanix.Persistence;
using Semanix.Persistence.Repositories;
using Serilog;
using SlipFree.Api.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using static CustomHeaderFilter;

var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;

// Add services to the container.
Log.Logger = SerilogConfig.Configure();
builder.Host.UseSerilog();
builder.Services.AddLogging(opt =>
{
    opt.AddConsole();
    opt.AddDebug();
});
builder.Services.AddCors();
builder.Services.AddResponseCompression();
builder.Services.AddRepository();

var connectionString = builder.Configuration.GetConnectionString("SemanixDbContext");

builder.Services.AddDbContext<SemanixDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SemanixDbContext")));

builder.Services.AddScoped<ISemanixDbContext>(provider =>
    provider.GetRequiredService<SemanixDbContext>());

builder.Services.AddRepository();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(APIRoot).Assembly));
builder.Services.AddMediatR(typeof(CreateTicketCommandHandler).Assembly);
//builder.Services.AddMediatR(typeof(GetTicketsByTenantQueryHandler).Assembly);
builder.Services.AddMediatR(typeof(APIRoot).Assembly);

builder.Services.AddHostedService<SlaMonitorService>();

builder.Services.AddHttpContextAccessor();
// Dapper — Add a scoped DB connection
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("SemanixDbContext");
    return new SqlConnection(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Semanix Ticketing Service", Version = "v1" });
    c.UseDateOnlyTimeOnlyStringConverters();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert the bearer token into this field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    c.OperationFilter<AddRequiredHeaderParameter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAppClientSettings(_config);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
    options.SuppressInferBindingSourcesForParameters = false;
    options.SuppressConsumesConstraintForFormFileParameters = true;
});

#region JWT Token

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(30),
        ValidateIssuerSigningKey = true,
        //ValidIssuer = authSettings?.ApiUrl,
        //ValidAudience = authSettings?.Audiences,
        //IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

#endregion

#region Redis Connection Configuration

#endregion

var app = builder.Build();

AppSettingsHelper.AppSettingsConfigure(app.Services.GetRequiredService<IConfiguration>());
// Configure the HTTP request pipeline.

app.UseStaticFiles();
app.UseDefaultFiles("/index.html");

if (!app.Configuration.GetValue<bool>("IsLive"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
}
else
{
    
}

app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyHeader();
    corsPolicyBuilder.AllowAnyMethod();
    corsPolicyBuilder.AllowAnyOrigin();
});

app.UseHeaderMiddleware();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.ConfigureCustomExceptionMiddleware();

//app.UseIpRateLimiting();

app.Run();


public class CustomHeaderFilter
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Tenant-Id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Client Identifier (Required)"
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Entity-Id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Entity Identifier (Optional)"
            });
        }
    }
}


public class HeaderMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        //var fullUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        var Host = $"{context.Request.Host}";
        var path = $"{context.Request.Path}";

        if (path.Contains("HealthChecks"))
        { }
        else
        {
            // Check if the "Client-ID" header is present
            //if (!context.Request.Headers.ContainsKey("Client-Id"))
            if (!context.Request.Headers.ContainsKey("X-Tenant-Id"))
            {
                await WriteResponseAsync(context, "Invalid Tenant Id", 401, true);
                return;
            }
            // Extract client ID from the "Client-ID" header
            //var clientId = context.Request.Headers["Client-ID"].ToString();
            var clientId = context.Request.Headers["X-Tenant-Id"].ToString();
            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

            if (clientId != configuration["AppSettings:XTenantId"])
            {
                await WriteResponseAsync(context, "Invalid Tenant Id", 401, true);
                return;
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
    private async Task WriteResponseAsync(HttpContext context, string message, int code, bool hasError)
    {
        var output = new Response
        {
            StatusCode = code,
            Message = message,
        };

        string responseString = JsonConvert.SerializeObject(output);
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
        context.Response.StatusCode = 401;
        await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class HeaderMiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeaderMiddleware>();
    }
}
