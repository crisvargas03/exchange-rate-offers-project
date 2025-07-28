using Serilog.Extensions.Logging;

namespace OrchestratorMicroService.API.Loggin
{
    public static class SerilogConfigurator
    {
        public static void RegisterSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = LoggerFactory.CreateLogger(configuration);

            var factory = new SerilogLoggerFactory(logger, true);
            services.AddSingleton<ILoggerFactory>(factory);

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddProvider(new SerilogLoggerProvider(logger, true));
            });
        }
    }
}
