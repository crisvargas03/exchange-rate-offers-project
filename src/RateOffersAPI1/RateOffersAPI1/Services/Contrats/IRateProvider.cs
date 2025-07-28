namespace RateOffersAPI1.Services.Contrats
{
    public interface IRateProvider
    {
        decimal GetRate(string fromCurrency, string toCurrency);
    }
}
