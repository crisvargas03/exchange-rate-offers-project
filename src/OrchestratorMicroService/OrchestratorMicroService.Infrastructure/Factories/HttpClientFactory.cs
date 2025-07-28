using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Infrastructure.Configuration;
using OrchestratorMicroService.Infrastructure.Implementations;
using OrchestratorMicroService.Infrastructure.Interfaces;
using OrchestratorMicroService.Infrastructure.Providers;

namespace OrchestratorMicroService.Infrastructure.Factories
{
    public static class HttpClientFactory
    {
        private static void RegisterClient<T>(this IServiceCollection services, string baseUrl) where T : class, IExchangeRateProvider
        {
            services.AddHttpClient<T>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(5000);
            });

            services.AddScoped(typeof(IHttpRequestHandler<T>), typeof(HttpRequestHandler<T>));
            services.AddScoped<IExchangeRateProvider, T>();
        }

        public static void AddHttpClientsByFactory(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("ApiProviders").Get<ApiProviderSettings>()!;
            RegisterClient<Api1ExchangeRateProvider>(services, settings.Api1BaseUrl);

            // RegisterClient<Api2ExchangeRateProvider>(services, settings.Api2BaseUrl);
            // RegisterClient<Api3ExchangeRateProvider>(services, settings.Api3BaseUrl);
            // RegisterClient<ApiExternalExchangeRateProvider>(services, settings.ApiExternalBaseUrl);
        }
    }
}
