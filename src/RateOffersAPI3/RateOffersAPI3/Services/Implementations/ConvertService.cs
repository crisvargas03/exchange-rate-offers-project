using RateOffersAPI3.Models;
using RateOffersAPI3.Services.Contrats;

namespace RateOffersAPI3.Services.Implementations
{
    public class ConvertService : IConvertService
    {
        private readonly IRateProvider _rateProvider;

        public ConvertService(IRateProvider rateProvider)
        {
            _rateProvider = rateProvider;
        }

        public Tuple<decimal, List<string>> ConvertCurrency(Exchange exchange)
        {
            try
            {
                var errors = ValidateExchangeInput(exchange);
                if (errors.Count > 1)
                    return new Tuple<decimal, List<string>>(0, errors);

                var rate = _rateProvider.GetRate(exchange.SourceCurrency, exchange.TargetCurrency);
                var result = exchange.Quantity * rate;
                return new Tuple<decimal, List<string>>(result, []);

            }
            catch (InvalidOperationException ex)
            {
                return new Tuple<decimal, List<string>>(0, [ex.Message]);
            }
        }

        private static List<string> ValidateExchangeInput(Exchange exchange)
        {
            List<string> errors = [];
            if (exchange is null)
            {
                errors.Add("Exchange input cannot be null");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(exchange.SourceCurrency))
                errors.Add("Source currency cannot be empty");

            if (string.IsNullOrWhiteSpace(exchange.TargetCurrency))
                errors.Add("Target currency cannot be empty");

            if (exchange.Quantity <= 0)
                errors.Add("Quantity must be greater than zero");

            if (exchange.SourceCurrency == exchange.TargetCurrency)
                errors.Add("Source and Target currencies cannot be the same");

            return errors;
        }
    }
}
