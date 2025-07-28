using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Application.Services
{
    public class BestExchangeRateService : IBestExchangeRateService
    {
        public Task<CurrencyResult> GetBestOfferAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
