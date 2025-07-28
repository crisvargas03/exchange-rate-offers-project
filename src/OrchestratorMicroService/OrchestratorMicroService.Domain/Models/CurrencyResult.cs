namespace OrchestratorMicroService.Domain.Models
{
    public class CurrencyResult
    {
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
