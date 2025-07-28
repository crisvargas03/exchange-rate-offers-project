namespace OrchestratorMicroService.Domain.Models
{
    public class CurrencyResult
    {
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public bool IsSuccessful { get; set; }

        private CurrencyResult() { }

        public static CurrencyResult Success(string provider, decimal amount, decimal rate)
        {
            return new CurrencyResult
            {
                Provider = provider,
                Amount = amount,
                Rate = rate,
                IsSuccessful = true
            };
        }

        public static CurrencyResult Fail(string provider)
        {
            return new CurrencyResult
            {
                Provider = provider,
                Amount = 0,
                Rate = 0,
                IsSuccessful = false
            };
        }
    }
}
