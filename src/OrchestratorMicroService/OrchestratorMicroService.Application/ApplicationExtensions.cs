using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Application.Options;
using OrchestratorMicroService.Application.Services;

namespace OrchestratorMicroService.Application
{
    public static class ApplicationExtensions
    {
        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBestExchangeRateService, BestExchangeRateService>();
        }

        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services
            services.AddApplicationServices();
            // Configure options
            // services.Configure<OrchestratorOptions>(configuration.GetSection("OrchestratorSettings"));
        }
    }
}
