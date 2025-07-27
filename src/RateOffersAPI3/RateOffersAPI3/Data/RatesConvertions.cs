using RateOffersAPI3.Models;

namespace RateOffersAPI3.Data
{
    public static class RatesConvertions
    {
        public static readonly Dictionary<CurrencyPair, List<decimal>> DummiesRates = new()
        {
            [new("DOP", "USD")] = [0.0168m, 0.0167m, 0.0169m],
            [new("USD", "DOP")] = [60.0m, 59.9m, 60.5m],

            [new("DOP", "EUR")] = [0.0144m, 0.0145m],
            [new("EUR", "DOP")] = [71.3m, 71.4m],

            [new("USD", "EUR")] = [0.85m, 0.86m, 0.84m],
            [new("EUR", "USD")] = [1.18m, 1.17m, 1.19m],

            [new("DOP", "CAD")] = [0.0125m, 0.0126m],
            [new("CAD", "DOP")] = [44.23m, 44.30m],

            [new("USD", "CAD")] = [1.25m, 1.24m],
            [new("CAD", "USD")] = [0.80m, 0.81m],
        };
    }
}
