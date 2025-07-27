namespace RateOffersAPI3.Models
{
    public class ExchangeRequest
    {
        public Exchange Exchange { get; set; } = new Exchange();
    }

    public class Exchange
    {
        public string SourceCurrency { get; set; } = string.Empty;
        public string TargetCurrency { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
