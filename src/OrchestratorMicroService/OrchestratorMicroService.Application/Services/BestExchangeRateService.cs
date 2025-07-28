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

            var timeout = _settings.ProviderTimeoutMilliseconds;
            var minToSuccessResponse = _settings.MinimumSuccessfulResponses;

            var tasks = _exchangeRateProviders
                .Select(provider => CallProvider(provider, request, cancellationToken))
                .ToList();

            var results = await Task.WhenAll(tasks);

            var validResults = results
                .Where(r => r is not null)
                .OrderByDescending(r => r!.Amount)
                .ToList();

            if (validResults.Count < minToSuccessResponse)
            {
                _logger.LogWarning("Only {Count} successful responses received. Minimum required is {Required}.", validResults.Count, minToSuccessResponse);
                return ApiResponse<CurrencyResult>.NotFound("Not enough valid provider responses.");
            }

            var best = validResults.FirstOrDefault();
            var bestOffer = CurrencyResult.Success(best!.Provider, best.Amount, best.Rate);
            return ApiResponse<CurrencyResult>.Success(bestOffer);
        }

        private async Task<CurrencyResult?> CallProvider(
            IExchangeRateProvider provider, 
            CurrencyRequest request,
            CancellationToken globalToken)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(globalToken);
            cts.CancelAfter(_settings.ProviderTimeoutMilliseconds);

            try
            {
                var result = await provider.GetExchangeRateAsync(request, cts.Token);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Provider {Provider} returned unsuccessful result.", result.Provider);
                    return null;
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Provider {Provider} timed out.", provider.GetType().Name);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling provider {Provider}.", provider.GetType().Name);
                return null;
            }
        }
    }
}
