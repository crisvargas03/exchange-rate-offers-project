using RateOffersAPI1.Data;
using RateOffersAPI1.Models;
using RateOffersAPI1.Services.Contrats;

namespace RateOffersAPI1.Services.Implementations
{
    public class RateProvider : IRateProvider
    {
        private readonly Random _random = new();
        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            var currencyPair = new CurrencyPair(fromCurrency, toCurrency);

            if (RatesConvertions.DummiesRates.TryGetValue(currencyPair, out var options))
                return options[_random.Next(options.Count)];

            throw new InvalidOperationException($"Unsupported currency pair: {currencyPair}");
        }
    }
}
