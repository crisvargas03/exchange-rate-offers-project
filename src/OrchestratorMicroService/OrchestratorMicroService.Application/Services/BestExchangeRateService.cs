using Microsoft.Extensions.Options;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Application.Options;
using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Application.Services
{
    public class BestExchangeRateService : IBestExchangeRateService
    {
        private readonly IEnumerable<IExchangeRateProvider> _exchangeRateProviders;
        private readonly OrchestratorOptions _settings;

        public BestExchangeRateService(IEnumerable<IExchangeRateProvider> exchangeRateProviders, IOptions<OrchestratorOptions> settings)
        {
            _exchangeRateProviders = exchangeRateProviders;
            _settings = settings.Value;
        }

        public async Task<CurrencyResult> GetBestOfferAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {

            using var timeoutCts = new CancellationTokenSource(_settings.ProviderTimeoutMilliseconds);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            var tasks = _exchangeRateProviders.Select(provider => CallProvider(provider, request, linkedCts.Token)).ToList();

            var results = await Task.WhenAll(tasks);

            var badResponse = new CurrencyResult
            {
                Provider = "",
                IsSuccessful = false,
                Amount = 0
            };

            var validResults = results
                .Where(r => r is not null)
                .OrderByDescending(r => r!.Amount)
                .ToList();

            if (!validResults.Any())
            {
                //_logger.LogWarning("No valid responses received from any provider.");
                return badResponse;
            }

            var best = validResults.First();

            return new CurrencyResult
            {
                Provider = best.Provider,
                Amount = best.Amount,
                Rate = best.Rate,
                IsSuccessful = true
            };
        }

        private async Task<CurrencyResult?> CallProvider(IExchangeRateProvider provider, CurrencyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await provider.GetExchangeRateAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex, "Provider {Provider} failed.", provider.GetType().Name);
                return null;
            }
        }
    } 
}
