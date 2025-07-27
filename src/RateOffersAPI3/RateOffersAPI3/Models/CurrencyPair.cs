namespace RateOffersAPI3.Models
{
    public record CurrencyPair (string From, string To)
    {
        public override string ToString() => $"{From}-{To}";
    }
}
