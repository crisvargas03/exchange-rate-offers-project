using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;
using OrchestratorMicroService.Infrastructure.Implementations;
using OrchestratorMicroService.Infrastructure.Interfaces;

namespace OrchestratorMicroService.Infrastructure.Providers
{
    public class Api1ExchangeRateProvider : IExchangeRateProvider
    {
        private const string ProviderName = "API1";
        private readonly IHttpRequestHandler<Api1ExchangeRateProvider> _httpClient;

        public Api1ExchangeRateProvider(IHttpRequestHandler<Api1ExchangeRateProvider> httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {

            var urlWithParams = $"/api/rates/exchanges?fromCurrency={request.SourceCurrency}&toCurrency={request.TargetCurrency}";
            var response = await _httpClient.GetAsync<Api1Response>(urlWithParams, cancellationToken);

            if (response is null)
            {
                return CurrencyResult.Fail(ProviderName);
            }

            return CurrencyResult.Success(ProviderName, request.Amount, response.Rate);

        }

        private class Api1Response
        {
            public decimal Rate { get; set; }
        }
    }
}
