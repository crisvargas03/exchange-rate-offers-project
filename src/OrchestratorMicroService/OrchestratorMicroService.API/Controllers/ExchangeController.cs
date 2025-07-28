using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrchestratorMicroService.API.Models;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.API.Controllers
{
    [Route("api/exchange")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IBestExchangeRateService _bestExchangeRateService;
        private readonly IValidator<CurrencyRequestDto> _currencyRequestValidator;
        public ExchangeController(IBestExchangeRateService bestExchangeRateService, IValidator<CurrencyRequestDto> currencyRequestValidator)
        {
            _bestExchangeRateService = bestExchangeRateService;
            _currencyRequestValidator = currencyRequestValidator;
        }

        [HttpPost]
        public async Task<IActionResult> GetBestRate([FromBody]CurrencyRequestDto request, CancellationToken cancellationToken)
        {

            var validationResult = await _currencyRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var badresponse = ApiResponse<CurrencyRequest>.Fail(errors, System.Net.HttpStatusCode.BadRequest);
                badresponse.Message = "Validation Failded";
                return BadRequest(badresponse);
            }

            var currencyRequest = new CurrencyRequest
            {
                SourceCurrency = request.SourceCurrency,
                TargetCurrency = request.TargetCurrency,
                Amount = request.Amount
            };

            var result = await _bestExchangeRateService.GetBestOfferAsync(currencyRequest, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var notFoundResponse = ApiResponse<CurrencyRequest>.NotFound("No exchange rate found for the provided currencies.");
                return NotFound(notFoundResponse);
            }

            return Ok(result);
        }
    }
}
