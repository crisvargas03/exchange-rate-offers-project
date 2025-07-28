using Microsoft.AspNetCore.Mvc;
using RateOffersAPI3.Models;
using RateOffersAPI3.Services.Contrats;

namespace RateOffersAPI3.Controllers
{
    public static class RatesExchangesEndpoints
    {
        public static void MapJsonRateEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/rates/custom-response", ([FromBody] ExchangeRequest request, [FromServices] IConvertService convertService) =>
            {
                var result = convertService.ConvertCurrency(request.Exchange);
                if (result.Item2.Count > 0)
                {
                    return Results.BadRequest(new ApiResponse<List<string>>
                    {
                        Data = result.Item2,
                        Message = "No valid exchange rates found.",
                        StatusCode = 400
                    });
                }

                return Results.Ok(new ApiResponse<object>
                {
                    Data = new { Total = result.Item1 },
                    Message = "Exchange rate calculated successfully.",
                    StatusCode = 200
                });
            });
        }
    }
}
