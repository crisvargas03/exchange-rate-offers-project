using RateOffersAPI3.Models;

namespace RateOffersAPI3.Data
{
    public static class RatesConvertions
    {
        public static readonly Dictionary<CurrencyPair, List<decimal>> DummiesRates = new()
        {
            [new("DOP", "USD")] = [0.0167m],
            [new("USD", "DOP")] = [59.9m],

            [new("DOP", "EUR")] = [0.0145m],
            [new("EUR", "DOP")] = [71.4m],

            [new("USD", "EUR")] = [0.86m],
            [new("EUR", "USD")] = [1.17m],

            [new("DOP", "CAD")] = [0.0126m],
            [new("CAD", "DOP")] = [44.30m],

            [new("USD", "CAD")] = [1.24m],
            [new("CAD", "USD")] = [0.81m],
        };
    }
}
