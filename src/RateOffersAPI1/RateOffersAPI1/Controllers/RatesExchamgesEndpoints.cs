using Microsoft.AspNetCore.Mvc;
using RateOffersAPI1.Services.Contrats;

namespace RateOffersAPI1.Controllers
{
    public static class RatesExchamgesEndpoints
    {
        public static void MapRatesExchamgesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/rates/exchanges", async (string fromCurrency, string toCurrency, [FromServices] IRateProvider rateProvider) =>
            {
                if (string.IsNullOrWhiteSpace(fromCurrency)
                    || string.IsNullOrWhiteSpace(toCurrency))
                {
                    return Results.BadRequest("Both 'fromCurrency' and 'toCurrency' must be provided.");
                }
                try
                {
                    await Task.Delay(3000);
                    var rate = rateProvider.GetRate(fromCurrency, toCurrency);
                    return Results.Ok(new { rate });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
        }
    }
}
