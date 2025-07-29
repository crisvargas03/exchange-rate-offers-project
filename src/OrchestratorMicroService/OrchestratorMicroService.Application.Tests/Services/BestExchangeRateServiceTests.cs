using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchestratorMicroService.Application.Options;
using OrchestratorMicroService.Application.Services;
using OrchestratorMicroService.Application.Interfaces;
using OrchestratorMicroService.Domain.Models;

namespace OrchestratorMicroService.Application.Tests.Services;

public class BestExchangeRateServiceTests
{
    private readonly Mock<ILogger<BestExchangeRateService>> _loggerMock = new();
    private readonly OrchestratorOptions _options = new()
    {
        MinimumSuccessfulResponses = 2,
        ProviderTimeoutMilliseconds = 1000
    };

    private BestExchangeRateService CreateService(params IExchangeRateProvider[] providers)
    {
        var optionsMock = new Mock<IOptions<OrchestratorOptions>>();
        optionsMock.Setup(o => o.Value).Returns(_options);

        return new BestExchangeRateService(providers, optionsMock.Object, _loggerMock.Object);
    }

    private static CurrencyRequest DummyRequest => new() { SourceCurrency = "USD", TargetCurrency = "DOP", Amount = 100 };

    [Fact]
    public async Task Should_Return_Best_Offer_Among_Multiple_Valid()
    {
        var p1 = new Mock<IExchangeRateProvider>();
        p1.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P1", 5600, 56));

        var p2 = new Mock<IExchangeRateProvider>();
        p2.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P2", 5800, 58));

        var service = CreateService(p1.Object, p2.Object);

        var result = await service.GetBestOfferAsync(DummyRequest, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Provider.Should().Be("P2");
        result.Data.Amount.Should().Be(5800);
    }

    [Fact]
    public async Task Should_Ignore_Null_Results_From_Providers()
    {
        var p1 = new Mock<IExchangeRateProvider>();
        p1.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((CurrencyResult?)null);

        var p2 = new Mock<IExchangeRateProvider>();
        p2.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P2", 5700, 57));

        var p3 = new Mock<IExchangeRateProvider>();
        p3.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((CurrencyResult?)null);

        var service = CreateService(p1.Object, p2.Object, p3.Object);

        var result = await service.GetBestOfferAsync(DummyRequest, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Not enough");
    }

    [Fact]
    public async Task Should_Throw_If_Valid_Responses_Less_Than_Minimum()
    {
        var p1 = new Mock<IExchangeRateProvider>();
        p1.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((CurrencyResult?)null);

        var p2 = new Mock<IExchangeRateProvider>();
        p2.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P2", 5700, 57));

        var p3 = new Mock<IExchangeRateProvider>();
        p3.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((CurrencyResult?)null);

        var service = CreateService(p1.Object, p2.Object, p3.Object);

        var result = await service.GetBestOfferAsync(DummyRequest, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Not enough");
    }

    [Fact]
    public async Task Should_Order_By_Highest_Amount()
    {
        var p1 = new Mock<IExchangeRateProvider>();
        p1.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P1", 5400, 54));

        var p2 = new Mock<IExchangeRateProvider>();
        p2.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P2", 5900, 59));

        var p3 = new Mock<IExchangeRateProvider>();
        p3.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(CurrencyResult.Success("P3", 5700, 57));

        var service = CreateService(p1.Object, p2.Object, p3.Object);

        var result = await service.GetBestOfferAsync(DummyRequest, CancellationToken.None);
        
        result.Data!.Provider.Should().Be("P2");
    }

    [Fact]
    public async Task Should_Call_All_Providers()
    {
        var mocks = Enumerable.Range(1, 3).Select(i =>
        {
            var m = new Mock<IExchangeRateProvider>();
            m.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(CurrencyResult.Success($"P{i}", 5000 + i * 100, 50 + i));
            return m;
        }).ToList();

        var service = CreateService(mocks.Select(m => m.Object).ToArray());

        await service.GetBestOfferAsync(DummyRequest, CancellationToken.None);

        foreach (var mock in mocks)
        {
            mock.Verify(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Fact]
    public async Task Should_Handle_Exceptions_And_Continue()
    {
        var failing = new Mock<IExchangeRateProvider>();
        failing.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception("Simulated failure"));

        var valid = new Mock<IExchangeRateProvider>();
        valid.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(CurrencyResult.Success("OK", 5800, 58));

        var valid2 = new Mock<IExchangeRateProvider>();
        valid2.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyRequest>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(CurrencyResult.Success("OK", 5800, 58));

        var service = CreateService(failing.Object, valid.Object, valid2.Object);

        var result = await service.GetBestOfferAsync(DummyRequest, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Provider.Should().Be("OK");
    }
}
