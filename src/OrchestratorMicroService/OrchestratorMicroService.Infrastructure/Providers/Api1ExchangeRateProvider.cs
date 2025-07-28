using Microsoft.Extensions.Logging;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;
using System.Net.Http.Json;

namespace OrchestratorMicroService.Infrastructure.Providers
{
    public class Api1ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Api1ExchangeRateProvider> _logger;

        public Api1ExchangeRateProvider(HttpClient httpClient, ILogger<Api1ExchangeRateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var urlWithParams = $"/api/rates/exchanges?fromCurrency={request.SourceCurrency}&toCurrency={request.TargetCurrency}";
                Console.WriteLine(_httpClient.BaseAddress);
                var response = await _httpClient.GetAsync(urlWithParams, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return new CurrencyResult
                    {
                        Provider = "API1",
                        IsSuccessful = false,
                        Amount = 0,
                        Rate = 0
                    };

                }
               
                var content = await response.Content.ReadFromJsonAsync<Api1Response>(cancellationToken: cancellationToken);
                if (content is null)
                {
                    _logger.LogWarning("Received null response from API1 for request: {Request}", request);
                    return new CurrencyResult
                    {
                        Provider = "API1",
                        IsSuccessful = false,
                        Amount = 0,
                        Rate = 0
                    };
                }

                var result = new CurrencyResult
                {
                    Provider = "API1",
                    Rate = content.Rate,
                    Amount = content.Rate * request.Amount,
                    IsSuccessful = true
                };
                return result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "Request to API1 timed out for request: {Request}", request);
                return new CurrencyResult
                {
                    Provider = "API1",
                    IsSuccessful = false,
                    Amount = 0,
                    Rate = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling API1 for request: {Request}", request);
                return new CurrencyResult
                {
                    Provider = "API1",
                    IsSuccessful = false,
                    Amount = 0,
                    Rate = 0
                };
            }
        }

        private class Api1Response
        {
            public decimal Rate { get; set; }
        }
    }
}
