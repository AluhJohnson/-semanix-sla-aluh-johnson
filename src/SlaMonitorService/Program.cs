using SlaMonitorService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Use Prometheus metrics endpoint
var app = builder.Build();
app.UseMetricServer();
app.UseHttpMetrics();

var host = builder.Build();
host.Run();
