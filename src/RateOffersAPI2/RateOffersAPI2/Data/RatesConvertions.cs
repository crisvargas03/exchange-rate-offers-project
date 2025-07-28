using RateOffersAPI2.Models;

namespace RateOffersAPI2.Data
{
    public static class RatesConvertions
    {
        public static readonly Dictionary<CurrencyPair, List<decimal>> DummiesRates = new()
        {
            [new("DOP", "USD")] = [0.0168m],
            [new("USD", "DOP")] = [60.2m],

            [new("DOP", "EUR")] = [0.0144m],
            [new("EUR", "DOP")] = [71.2m],

            [new("USD", "EUR")] = [0.86m],
            [new("EUR", "USD")] = [1.19m],

            [new("USD", "CAD")] = [1.24m],
            [new("CAD", "USD")] = [0.81m],
        };
    }
}
