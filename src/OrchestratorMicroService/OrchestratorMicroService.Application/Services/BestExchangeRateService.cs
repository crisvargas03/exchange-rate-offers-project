using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BestExchangeRateService> _logger;

        public BestExchangeRateService(IEnumerable<IExchangeRateProvider> exchangeRateProviders, IOptions<OrchestratorOptions> settings, ILogger<BestExchangeRateService> logger)
        {
            _exchangeRateProviders = exchangeRateProviders;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ApiResponse<CurrencyResult>> GetBestOfferAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {

            using var timeoutCts = new CancellationTokenSource(_settings.ProviderTimeoutMilliseconds);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            var tasks = _exchangeRateProviders.Select(provider => CallProvider(provider, request, linkedCts.Token)).ToList();

            var results = await Task.WhenAll(tasks);

            var validResults = results
                .Where(r => r is not null)
                .OrderByDescending(r => r!.Amount)
                .ToList();

            if (validResults.Count == 0)
            {
                _logger.LogWarning("No valid responses received from any provider.");
                return ApiResponse<CurrencyResult>.NotFound("No results for Any Providers");
            }

            var best = validResults.FirstOrDefault();

            var bestOffer = CurrencyResult.Success(best!.Provider, best.Amount, best.Rate);

            return ApiResponse<CurrencyResult>.Success(bestOffer);
        }

        private async Task<CurrencyResult?> CallProvider(IExchangeRateProvider provider, CurrencyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var providerResult = await provider.GetExchangeRateAsync(request, cancellationToken);
                if (!providerResult.IsSuccessful)
                {
                    _logger.LogWarning("Provider {Provider} returned an unsuccessful result for request: {Request}", providerResult.Provider, request);
                    return null;
                }
                return providerResult;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Provider {Provider} failed.", provider.GetType().Name);
                return null;
            }
        }
    } 
}
