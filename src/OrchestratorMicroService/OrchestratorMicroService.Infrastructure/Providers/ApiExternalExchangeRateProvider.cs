using Microsoft.Extensions.Options;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Application.Options;
using OrchestratorMicroService.Domain.Models;
using OrchestratorMicroService.Infrastructure.Interfaces;

namespace OrchestratorMicroService.Infrastructure.Providers
{
    public class ApiExternalExchangeRateProvider : IExchangeRateProvider
    {
        private const string ProviderName = "APIExternal";
        private readonly IHttpRequestHandler<ApiExternalExchangeRateProvider> _httpClient;
        private readonly OrchestratorOptions _settings;
        public ApiExternalExchangeRateProvider(IHttpRequestHandler<ApiExternalExchangeRateProvider> httpClient, IOptions<OrchestratorOptions> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }
        public async Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ExternalApiKey))
            {
                return CurrencyResult.Fail(ProviderName);
                throw new InvalidOperationException("External API key is not configured.");
            }
            var urlWithParams = 
                $"/convert?from={request.SourceCurrency}&to={request.TargetCurrency}&amount={request.Amount}&api_key={_settings.ExternalApiKey}";
            var response = await _httpClient.GetAsync<ApiExternalResponse>(urlWithParams, cancellationToken);
            if (response is null)
            {
                return CurrencyResult.Fail(ProviderName);
            }
            return CurrencyResult.Success(ProviderName, request.Amount, response.Result.Rate);
        }
        private class ApiExternalResponse
        {
            public string Base { get; set; } = string.Empty;
            public decimal Amount { get; set; }
            public int Ms { get; set; }
            public Result Result { get; set; } = new Result();

        }
        private class Result
        {
            public decimal Rate { get; set; }
        }
    }
}
