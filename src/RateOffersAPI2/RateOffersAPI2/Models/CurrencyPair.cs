namespace RateOffersAPI2.Models
{
    public record CurrencyPair (string From, string To)
    {
        public override string ToString() => $"{From}-{To}";
    }
}
