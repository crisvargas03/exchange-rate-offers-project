using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratorMicroService.Infrastructure.Factories;

namespace OrchestratorMicroService.Infrastructure
{
    public static class InfraestructureExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClientsByFactory(configuration);
        }
    }
}
