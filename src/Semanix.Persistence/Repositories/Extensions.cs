using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Semanix.Application.BaseHelpers;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Domain;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Persistence.Repositories
{
    public static class Extensions
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<HttpContextServiceClient>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAlertRepository, AlertRepository>();
            services.AddTransient<ITicketRepository, TicketRepository>();

            //services.AddTransient<IErrorLogger, ErrorLoggerService>();

        }
    }
}
