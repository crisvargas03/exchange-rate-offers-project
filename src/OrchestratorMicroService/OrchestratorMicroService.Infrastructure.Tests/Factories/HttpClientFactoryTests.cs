using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Infrastructure.Factories;
using OrchestratorMicroService.Infrastructure.Interfaces;
using OrchestratorMicroService.Infrastructure.Providers;

namespace OrchestratorMicroService.Infrastructure.Tests.Factories;

public class HttpClientFactoryTests
{
    private IServiceProvider BuildServiceProvider(Action<IServiceCollection> configureServices, Dictionary<string, string>? settings = null)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings ?? new Dictionary<string, string>())
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(config);

        configureServices(services);

        return services.BuildServiceProvider();
    }

    [Fact]
    public void Should_Register_HttpClient_With_BaseAddress_And_Timeout()
    {
        // Arrange
        var baseUrls = new Dictionary<string, string>
        {
            ["ApiProviders:Api1BaseUrl"] = "https://api1.test/",
            ["ApiProviders:Api2BaseUrl"] = "https://api2.test/",
            ["ApiProviders:Api3BaseUrl"] = "https://api3.test/",
            ["ApiProviders:ApiExternalBaseUrl"] = "https://external.test/"
        };

        var provider = BuildServiceProvider(services =>
        {
            services.AddHttpClientsByFactory(new ConfigurationBuilder().AddInMemoryCollection(baseUrls).Build());
        }, baseUrls);

        // Act
        var factory = provider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient(nameof(Api1ExchangeRateProvider));

        // Assert
        client.Should().NotBeNull();
        client.BaseAddress!.ToString().Should().Be("https://api1.test/");
        client.Timeout.Should().Be(TimeSpan.FromMilliseconds(5000));
    }

    [Fact]
    public void Should_Resolve_All_IHttpRequestHandler_And_Providers()
    {
        var baseUrls = new Dictionary<string, string>
        {
            ["ApiProviders:Api1BaseUrl"] = "https://api1.test/",
            ["ApiProviders:Api2BaseUrl"] = "https://api2.test/",
            ["ApiProviders:Api3BaseUrl"] = "https://api3.test/",
            ["ApiProviders:ApiExternalBaseUrl"] = "https://external.test/"
        };

        var provider = BuildServiceProvider(services =>
        {
            services.AddHttpClientsByFactory(new ConfigurationBuilder().AddInMemoryCollection(baseUrls).Build());
        }, baseUrls);

        // Act & Assert
        provider.GetRequiredService<IHttpRequestHandler<Api1ExchangeRateProvider>>().Should().NotBeNull();
        provider.GetRequiredService<IHttpRequestHandler<Api2ExchangeRateProvider>>().Should().NotBeNull();
        provider.GetRequiredService<IHttpRequestHandler<Api3ExchangeRateProvider>>().Should().NotBeNull();
        provider.GetRequiredService<IHttpRequestHandler<ApiExternalExchangeRateProvider>>().Should().NotBeNull();

        provider.GetServices<IExchangeRateProvider>().Should().ContainSingle(p => p is Api1ExchangeRateProvider);
        provider.GetServices<IExchangeRateProvider>().Should().ContainSingle(p => p is Api2ExchangeRateProvider);
        provider.GetServices<IExchangeRateProvider>().Should().ContainSingle(p => p is Api3ExchangeRateProvider);
        provider.GetServices<IExchangeRateProvider>().Should().ContainSingle(p => p is ApiExternalExchangeRateProvider);
    }
}
