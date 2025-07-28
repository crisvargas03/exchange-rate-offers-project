namespace OrchestratorMicroService.API.Models
{
    public class CurrencyResponseDto
    {
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
    }
}
