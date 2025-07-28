using Microsoft.AspNetCore.Mvc;
using OrchestratorMicroService.API.Models;
using OrchestratorMicroService.Application.Interfaces;

namespace OrchestratorMicroService.API.Controllers
{
    [Route("api/exchange")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IBestExchangeRateService _bestExchangeRateService;
        public ExchangeController(IBestExchangeRateService bestExchangeRateService)
        {
            _bestExchangeRateService = bestExchangeRateService;
        }

        [HttpPost]
        public async Task<IActionResult> GetBestRate([FromBody]CurrencyRequestDto request, CancellationToken cancellationToken)
        {
            var currencyRequest = new Domain.Models.CurrencyRequest
            {
                SourceCurrency = request.SourceCurrency,
                TargetCurrency = request.TargetCurrency,
                Amount = request.Amount
            };

            var result = await _bestExchangeRateService.GetBestOfferAsync(currencyRequest, cancellationToken);
            if (result is null) 
                return NotFound("No exchange rate found for the provided currencies.");

            return Ok(result);
        }
    }
}
