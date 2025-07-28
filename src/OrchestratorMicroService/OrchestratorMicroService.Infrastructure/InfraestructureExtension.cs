using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Infrastructure.Providers;
using OrchestratorMicroService.Infrastructure.Factories;

namespace OrchestratorMicroService.Infrastructure
{
    public static class InfraestructureExtension
    {
        private static void AddServices(this IServiceCollection services)
        {
            // services.AddScoped<IExchangeRateProvider, Api1ExchangeRateProvider>();
        }
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services
            services.AddServices();
            services.AddHttpClientsByFactory(configuration);
        }
    }
}
