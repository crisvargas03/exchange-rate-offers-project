using RateOffersAPI2.Models;

namespace RateOffersAPI2.Services.Contrats
{
    public interface IConverterServices
    {
        Tuple<decimal, List<string>> ConvertCurrency(ExchangeInput exchange);
    }
}
