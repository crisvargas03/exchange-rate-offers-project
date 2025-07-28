using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Application.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken);
    }
}
