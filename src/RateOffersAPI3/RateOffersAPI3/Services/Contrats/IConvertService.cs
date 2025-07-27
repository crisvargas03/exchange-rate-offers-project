using RateOffersAPI3.Models;

namespace RateOffersAPI3.Services.Contrats
{
    public interface IConvertService
    {
        Tuple<decimal, List<string>> ConvertCurrency(Exchange exchange);
    }
}
