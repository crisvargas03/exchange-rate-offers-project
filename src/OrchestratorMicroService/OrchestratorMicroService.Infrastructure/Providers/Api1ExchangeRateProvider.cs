using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Infrastructure.Providers
{
    public class Api1ExchangeRateProvider : IExchangeRateProvider
    {
        public Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
