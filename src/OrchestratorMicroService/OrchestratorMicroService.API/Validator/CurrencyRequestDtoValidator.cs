using FluentValidation;
using OrchestratorMicroService.API.Models;

namespace OrchestratorMicroService.API.Validator
{
    public class CurrencyRequestDtoValidator : AbstractValidator<CurrencyRequestDto>
    {
        public CurrencyRequestDtoValidator()
        {
            RuleFor(x => x.SourceCurrency)
               .NotEmpty().WithMessage("Source currency is required.");

            RuleFor(x => x.TargetCurrency)
                .NotEmpty().WithMessage("Target currency is required.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
