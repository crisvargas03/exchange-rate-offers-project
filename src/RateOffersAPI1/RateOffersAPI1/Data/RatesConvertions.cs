using RateOffersAPI1.Models;

namespace RateOffersAPI1.Data
{
    public static class RatesConvertions
    {
        public static readonly Dictionary<CurrencyPair, List<decimal>> DummiesRates = new()
        {
            [new("DOP", "USD")] = [0.0167m, 0.0167m, 0.0166m],
            [new("USD", "DOP")] = [60.0m, 59.8m, 60.4m],
            [new("DOP", "EUR")] = [0.014m, 0.0141m],
            [new("EUR", "DOP")] = [70.0m, 71.0m],
        };
    }
}
