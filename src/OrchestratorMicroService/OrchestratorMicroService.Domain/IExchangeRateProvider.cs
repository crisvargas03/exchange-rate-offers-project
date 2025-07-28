using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Domain
{
    public interface IExchangeRateProvider
    {
        Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request);
    }
}
