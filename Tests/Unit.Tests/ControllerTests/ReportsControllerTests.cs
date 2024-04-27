
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.API.Controllers;

namespace Cargill.Reconc.UnitTests;

public class ReportsControllerTests
{
    private readonly ReportsController _controller;
    private readonly Mock<ILogger<ReportsController>> _mockLogger;
    private readonly Mock<IReconcCalculator> _mockCalculator;
    private readonly Mock<ITradingBusinessLogic> _mockTradingBusiness;
    private readonly Mock<IInsuranceBusinessLogic> _mockInsuranceBusiness;
    private readonly Mock<ICounterpartyBusinessLogic> _mockCounterpartiesBusiness;

    public ReportsControllerTests()
    {
        _mockLogger = new Mock<ILogger<ReportsController>>();
        _mockCalculator = new Mock<IReconcCalculator>();
        _mockTradingBusiness = new Mock<ITradingBusinessLogic>();
        _mockInsuranceBusiness = new Mock<IInsuranceBusinessLogic>();
        _mockCounterpartiesBusiness = new Mock<ICounterpartyBusinessLogic>();

        _controller = new ReportsController(
            _mockLogger.Object,
            _mockCalculator.Object,
            _mockTradingBusiness.Object,
            _mockInsuranceBusiness.Object,
            _mockCounterpartiesBusiness.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_OnSuccessfulRetrieval()
    {
        // Arrange
        var mockTradings = new Trading[4];
        var mockCounterparties = new Counterparty[2];
        var mockInsurance = new Insurance[2];

        _mockTradingBusiness.Setup(b => b.GetAll()).ReturnsAsync(mockTradings);
        _mockCounterpartiesBusiness.Setup(b => b.GetAll()).ReturnsAsync(mockCounterparties);
        _mockInsuranceBusiness.Setup(b => b.GetAll()).ReturnsAsync(mockInsurance);
        _mockCalculator.Setup(c => c.GetBatchReports(It.IsAny<Trading[]>(), It.IsAny<Counterparty[]>(), It.IsAny<Insurance[]>())).ReturnsAsync(new Report[4]);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithEmptyLists()
    {
        // Arrange
        _mockTradingBusiness.Setup(b => b.GetAll()).ReturnsAsync(new Trading[0]);
        _mockCounterpartiesBusiness.Setup(b => b.GetAll()).ReturnsAsync(new Counterparty[3]);
        _mockInsuranceBusiness.Setup(b => b.GetAll()).ReturnsAsync(new Insurance[2]);
        _mockCalculator.Setup(c => c.GetBatchReports(It.IsAny<Trading[]>(), It.IsAny<Counterparty[]>(), It.IsAny<Insurance[]>())).ReturnsAsync(new Report[0]);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsType<Report[]>(okResult?.Value);
        Assert.Empty((Report[])okResult.Value);
    }

    [Fact]
    public async Task GetByTradingId_ReturnsNotFound_ForMissingTrading()
    {
        // Arrange
        int tradingId = 1;
        _mockTradingBusiness.Setup(b => b.AggregateBySupplier(tradingId)).ReturnsAsync(() => null);

        // Act
        var result = await _controller.GetByTradingId(tradingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByTradingId_HandlesMissingPDRate()
    {
        // Arrange
        int tradingId = 1;
        var mockTradingAggregation = new TradingAggregation();
        var mockInsurance = new Insurance();

        _mockTradingBusiness.Setup(b => b.AggregateBySupplier(tradingId)).ReturnsAsync(mockTradingAggregation);
        _mockInsuranceBusiness.Setup(b => b.GetById(tradingId)).ReturnsAsync(mockInsurance);
        _mockCounterpartiesBusiness.Setup(b => b.GetPDRate(mockTradingAggregation.SupplierCode)).ReturnsAsync(() => null);
        _mockCalculator.Setup(c => c.GetReportByTrading(It.IsAny<TradingAggregation>(), It.IsAny<Insurance>(), 0)).Returns(() => new Report());

        // Act
        var result = await _controller.GetByTradingId(tradingId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}