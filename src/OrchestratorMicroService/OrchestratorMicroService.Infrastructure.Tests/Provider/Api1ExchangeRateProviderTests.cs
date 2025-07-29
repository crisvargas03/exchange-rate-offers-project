using FluentAssertions;
using Moq;
using OrchestratorMicroService.Domain.Models;
using OrchestratorMicroService.Infrastructure.Interfaces;
using OrchestratorMicroService.Infrastructure.Providers;

namespace OrchestratorMicroService.Infrastructure.Tests.Providers;

public class Api1ExchangeRateProviderTests
{
    private readonly Mock<IHttpRequestHandler<Api1ExchangeRateProvider>> _httpHandlerMock = new();

    private Api1ExchangeRateProvider CreateService()
    {
        return new Api1ExchangeRateProvider(_httpHandlerMock.Object);
    }

    private static CurrencyRequest DummyRequest => new()
    {
        SourceCurrency = "USD",
        TargetCurrency = "DOP",
        Amount = 100
    };

    [Fact]
    public async Task Should_Return_Valid_CurrencyResult_When_Response_Is_Not_Null()
    {
        // Arrange
        _httpHandlerMock
     .Setup(h => h.GetAsync<Api1ExchangeRateProvider.Api1Response>(
         It.IsAny<string>(), It.IsAny<CancellationToken>()))
     .ReturnsAsync(new Api1ExchangeRateProvider.Api1Response { Rate = 58.0m });

        var service = CreateService();

        // Act
        var result = await service.GetExchangeRateAsync(DummyRequest, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Provider.Should().Be("API1");
        result.Rate.Should().Be(58.0m);
        result.Amount.Should().Be(5800.000m); // 100 * 58.0 rounded
    }

    [Fact]
    public async Task Should_Return_Failed_CurrencyResult_When_Response_Is_Null()
    {
        _httpHandlerMock
            .Setup(h => h.GetAsync<Api1ExchangeRateProvider.Api1Response>(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Api1ExchangeRateProvider.Api1Response?)null);

        var service = CreateService();

        var result = await service.GetExchangeRateAsync(DummyRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.Provider.Should().Be("API1");
    }

    [Fact]
    public async Task Should_Throw_When_HttpHandler_Throws_Exception()
    {
        _httpHandlerMock
            .Setup(h => h.GetAsync<Api1ExchangeRateProvider.Api1Response>(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Simulated failure"));

        var service = CreateService();

        Func<Task> act = async () =>
            await service.GetExchangeRateAsync(DummyRequest, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Simulated failure");
    }

    [Fact]
    public async Task Should_Call_Correct_Url_With_Parameters()
    {
        string? calledUrl = null;

        _httpHandlerMock
            .Setup(h => h.GetAsync<Api1ExchangeRateProvider.Api1Response>(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, _) => calledUrl = url)
            .ReturnsAsync(new Api1ExchangeRateProvider.Api1Response { Rate = 57.0m });

        var service = CreateService();
        await service.GetExchangeRateAsync(DummyRequest, CancellationToken.None);

        calledUrl.Should().Contain("/api/rates/exchanges");
        calledUrl.Should().Contain("fromCurrency=USD");
        calledUrl.Should().Contain("toCurrency=DOP");
    }
}
