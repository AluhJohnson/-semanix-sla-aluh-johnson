using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Semanix.Application.Config;

public static class SerilogConfig
{
    public static Serilog.ILogger Configure()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("log/Semanix_logs.txt", rollingInterval: RollingInterval.Hour).CreateLogger();

        return Log.Logger;
    }
}