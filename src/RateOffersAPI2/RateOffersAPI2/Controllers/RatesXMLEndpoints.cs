using Microsoft.AspNetCore.Mvc;
using RateOffersAPI2.Helpers;
using RateOffersAPI2.Models;
using RateOffersAPI2.Services.Contrats;

namespace RateOffersAPI2.Controllers
{
    public static class RatesXMLEndpoints
    {
        public static void MapRatesXMLEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/rates/xml", async (HttpRequest request, [FromServices] IConverterServices converterServices) =>
            {
                var input = await XmlHelper.DeserializeFromBodyAsync<ExchangeInput>(request);
                var result = converterServices.ConvertCurrency(input);
                if (result.Item2.Count > 0)
                {
                    var errorResponse = new XmlExchangeResponse
                    {
                        Errors = new Error { Errors = result.Item2 },
                        Amount = 0
                    };
                    XmlResult<XmlExchangeResponse> xmlErrorResult = new(errorResponse);
                    return xmlErrorResult;
                }
                
                var response = new XmlExchangeResponse
                {
                    Amount = result.Item1
                };
                XmlResult<XmlExchangeResponse> xmlResult = new(response);
                return xmlResult;

            }).Accepts<ExchangeInput>("application/xml")
            .Produces<XmlResult<XmlExchangeResponse>>();
        }
    }
}
