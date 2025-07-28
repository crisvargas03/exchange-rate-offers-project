using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;
using OrchestratorMicroService.Infrastructure.Interfaces;

namespace OrchestratorMicroService.Infrastructure.Providers
{
    public class Api3ExchangeRateProvider : IExchangeRateProvider
    {
        private const string ProviderName = "API3";
        private readonly IHttpRequestHandler<Api3ExchangeRateProvider> _httpClient;

        public Api3ExchangeRateProvider(IHttpRequestHandler<Api3ExchangeRateProvider> httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {
            var requestBody = new ApiBody
            {
                exchange = new Exchange
                {
                    sourceCurrency = request.SourceCurrency,
                    targetCurrency = request.TargetCurrency,
                    quantity = request.Amount
                }
            };

            var url = "rates/custom-response";
            var response = await _httpClient.PostAsync<ApiBody, Api3Response>(url, requestBody, cancellationToken);

            if (response == null)
            {
                return CurrencyResult.Fail(ProviderName);
            }
            if (response.StatusCode != 200)
            {
                return CurrencyResult.Fail(ProviderName);
            }
            var rate = response.Data.Total / requestBody.exchange.quantity;
            return CurrencyResult.Success(ProviderName, response.Data.Total, rate);
        }

        private class Api3Response
        {
            public int StatusCode { get; set; }
            public string Message { get; set; } = string.Empty;
            public DataApi3 Data { get; set; } = new DataApi3();
        }
        private class DataApi3
        {
            public decimal Total { get; set; }
        }
        private class ApiBody
        {
            public Exchange exchange { get; set; } = new Exchange();
        }
        private class Exchange
        {
            public string sourceCurrency { get; set; } = string.Empty;
            public string targetCurrency { get; set; } = string.Empty;
            public decimal quantity { get; set; }
        }
    }
}
