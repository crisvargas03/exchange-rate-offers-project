using System.ComponentModel.DataAnnotations;

namespace OrchestratorMicroService.API.Models
{
    public class CurrencyRequestDto
    {
        [Required(ErrorMessage = "Source currency is required.")]
        public string SourceCurrency { get; set; } = string.Empty;
        [Required(ErrorMessage = "Target currency is required.")]
        public string TargetCurrency { get; set; } = string.Empty;
        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }
    }
}
