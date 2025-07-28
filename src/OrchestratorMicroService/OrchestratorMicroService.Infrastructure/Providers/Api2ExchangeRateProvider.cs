using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;
using OrchestratorMicroService.Infrastructure.Interfaces;
using System.Xml.Serialization;

namespace OrchestratorMicroService.Infrastructure.Providers
{
    public class Api2ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpRequestHandler<Api2ExchangeRateProvider> _httpClient;

        private const string ProviderName = "API2";
        public Api2ExchangeRateProvider(IHttpRequestHandler<Api2ExchangeRateProvider> httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyResult> GetExchangeRateAsync(CurrencyRequest request, CancellationToken cancellationToken)
        {
            var xmlBody = new ExchangeInput
            {
                From = request.SourceCurrency,
                To = request.TargetCurrency,
                Amount = request.Amount
            };

            var response = await _httpClient.PostXmlAsync<ExchangeInput, Result>("/api/rates/xml", xmlBody, cancellationToken);
            if (response is null)
            {
                return CurrencyResult.Fail(ProviderName);
            }
            var rate = response.Amount / request.Amount; // AmountConverted / amountToConvert
            return CurrencyResult.Success(ProviderName, response.Amount, rate);
        }


        [XmlRoot("ExchangeInput")]
        public class ExchangeInput
        {
            [XmlElement("from")]
            public string From { get; set; } = string.Empty;

            [XmlElement("to")]
            public string To { get; set; } = string.Empty;

            [XmlElement("amount")]
            public decimal Amount { get; set; }
        }

        [XmlRoot("Result")]
        public class Result
        {
            [XmlElement("amount")]
            public decimal Amount { get; set; }

            [XmlElement("errors")]
            public ErrorsContainer Errors { get; set; } = new();
        }

        public class ErrorsContainer
        {
            [XmlElement("Errors")]
            public string Errors { get; set; } = string.Empty;
        }
    }
}
