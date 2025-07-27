using RateOffersAPI2.Models;

namespace RateOffersAPI2.Data
{
    public static class RatesConvertions
    {
        public static readonly Dictionary<CurrencyPair, List<decimal>> DummiesRates = new()
        {
            [new("DOP", "USD")] = [0.0168m, 0.0169m, 0.0167m],
            [new("USD", "DOP")] = [60.2m, 59.9m, 60.1m],

            [new("DOP", "EUR")] = [0.0142m, 0.0144m],
            [new("EUR", "DOP")] = [71.1m, 71.2m],

            [new("USD", "EUR")] = [0.85m, 0.86m, 0.84m],
            [new("EUR", "USD")] = [1.18m, 1.17m, 1.19m],

            [new("USD", "CAD")] = [1.25m, 1.26m, 1.24m],
            [new("CAD", "USD")] = [0.80m, 0.81m, 0.79m],
        };
    }
}
