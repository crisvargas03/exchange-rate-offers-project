using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrchestratorMicroService.API.Controllers;
using OrchestratorMicroService.API.Models;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.API.Tests.Controllers;

public class ExchangeControllerTests
{
    private readonly Mock<IBestExchangeRateService> _bestRateServiceMock = new();
    private readonly Mock<IValidator<CurrencyRequestDto>> _validatorMock = new();

    private ExchangeController CreateController()
    {
        return new ExchangeController(_bestRateServiceMock.Object, _validatorMock.Object);
    }

    private static CurrencyRequestDto ValidDto => new()
    {
        SourceCurrency = "USD",
        TargetCurrency = "DOP",
        Amount = 100
    };

    [Fact]
    public async Task Should_Return_Ok_When_Request_Is_Valid()
    {
        // Arrange
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CurrencyRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var currencyResult = CurrencyResult.Success("API1", 100, 58.5m);
        var resultMock = ApiResponse<CurrencyResult>.Success(currencyResult);
        _bestRateServiceMock
            .Setup(s => s.GetBestOfferAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultMock);

        var controller = CreateController();

        // Act
        var response = await controller.GetBestRate(ValidDto, CancellationToken.None);

        // Assert
        var okResult = response as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var body = okResult.Value as ApiResponse<CurrencyResponseDto>;
        body!.IsSuccess.Should().BeTrue();
        body.Data!.Provider.Should().Be("API1");
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Validation_Fails()
    {
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("SourceCurrency", "Source is required")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CurrencyRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

        var controller = CreateController();

        var response = await controller.GetBestRate(ValidDto, CancellationToken.None);

        var badRequest = response as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);

        var body = badRequest.Value as ApiResponse<CurrencyResponseDto>;
        body!.IsSuccess.Should().BeFalse();
        body.Message.Should().Be("Validation Failded");
        body.Errors.Should().Contain("Source is required");
    }

    [Fact]
    public async Task Should_Return_NotFound_When_BestRateService_Returns_NotFound()
    {
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CurrencyRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var notFound = ApiResponse<CurrencyResult>.NotFound("No exchange rate found");

        _bestRateServiceMock
            .Setup(s => s.GetBestOfferAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(notFound);

        var controller = CreateController();

        var response = await controller.GetBestRate(ValidDto, CancellationToken.None);

        var notFoundResult = response as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);

        var body = notFoundResult.Value as ApiResponse<CurrencyResponseDto>;
        body!.IsSuccess.Should().BeFalse();
        body.Message.Should().Contain("No exchange rate found");
    }
}
