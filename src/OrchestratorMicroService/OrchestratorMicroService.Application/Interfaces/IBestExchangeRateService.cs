using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Application.Interfaces
{
    public interface IBestExchangeRateService
    {
        Task<CurrencyResult> GetBestOfferAsync(CurrencyRequest request, CancellationToken cancellationToken);
    }
}
