using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace OrchestratorMicroService.API.Loggin
{
    public static class LoggerFactory
    {
        public static Logger CreateLogger(IConfiguration configuration)
        {
            var logginPath = configuration["Logging:FilePath"] ?? "Logs/logs.txt";
            return new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.File(logginPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
