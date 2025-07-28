using RateOffersAPI2.Models;
using RateOffersAPI2.Services.Contrats;

namespace RateOffersAPI2.Services.Implementations
{
    public class ConverterServices : IConverterServices
    {
        private readonly IRateProvider _rateProvider;

        public ConverterServices(IRateProvider rateProvider)
        {
            _rateProvider = rateProvider;
        }

        public Tuple<decimal, List<string>> ConvertCurrency(ExchangeInput exchange)
        {
            try
            {
                var errors = ValidateExchangeInput(exchange);
                if (errors.Count > 1)
                    return new Tuple<decimal, List<string>>(0, errors);

                var rate = _rateProvider.GetRate(exchange.From, exchange.To);
                var result = exchange.Amount * rate;
                return new Tuple<decimal, List<string>>(result, []);

            }
            catch (InvalidOperationException ex)
            {
                return new Tuple<decimal, List<string>>(0, [ex.Message]);
            }
        }

        private static List<string> ValidateExchangeInput(ExchangeInput exchange)
        {
            List<string> errors = [];
            if (exchange is null)
            {
                errors.Add("Exchange input cannot be null");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(exchange.From))
                errors.Add("From currency cannot be empty");

            if (string.IsNullOrWhiteSpace(exchange.To))
                errors.Add("To currency cannot be empty");

            if (exchange.Amount <= 0)
                errors.Add("Amount must be greater than zero");

            if (exchange.From == exchange.To)
                errors.Add("From and To currencies cannot be the same");

            return errors;
        }
    }
}
