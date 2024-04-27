using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Web.API.Controllers;

namespace Cargill.Reconc.UnitTests;

public class TradingsControllerTests
{
    private readonly Mock<ITradingBusinessLogic> _mockBusiness;
    private readonly Mock<ILogger<TradingsController>> _mockLogger;
    private readonly TradingsController _controller;

    public TradingsControllerTests()
    {
        _mockBusiness = new Mock<ITradingBusinessLogic>();
        _mockLogger = new Mock<ILogger<TradingsController>>();
        _controller = new TradingsController(_mockLogger.Object, _mockBusiness.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithAllTradings()
    {
        // Arrange
        var expectedTradings = new [] { new Trading { Id = 1 }, new Trading { Id = 2 } };
        _mockBusiness.Setup(b => b.GetAll()).ReturnsAsync(() => expectedTradings);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(expectedTradings, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithEmptyList()
    {
        // Arrange
        _mockBusiness.Setup(b => b.GetAll()).ReturnsAsync(() => Array.Empty<Trading>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsType<Trading[]>(okResult.Value);
        Assert.Empty((Trading[])okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WithExistingTrading()
    {
        // Arrange
        var existingId = 1;
        var expectedTrading = new Trading { Id = existingId };
        _mockBusiness.Setup(b => b.GetById(existingId)).ReturnsAsync(() => expectedTrading);

        // Act
        var result = await _controller.GetById(existingId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(expectedTrading, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_ForNonexistentTrading()
    {
        // Arrange
        var nonExistentId = 10;
        _mockBusiness.Setup(b => b.GetById(nonExistentId)).ReturnsAsync(() => null);

        // Act
        var result = await _controller.GetById(nonExistentId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Add_ReturnsOk_OnSuccessfulAddition()
    {
        // Arrange
        var newTrading = new Trading { Id = 1 };
        _mockBusiness.Setup(b => b.AddTrading(newTrading)).ReturnsAsync(true);

        // Act
        var result = await _controller.Add(newTrading);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Add_ReturnsProblem_OnFailedAddition()
    {
        // Arrange
        var newTrading = new Trading { Id = 1 };
        _mockBusiness.Setup(b => b.AddTrading(newTrading)).ReturnsAsync(false);

        // Act
        var result = await _controller.Add(newTrading);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var problemResult = result as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, problemResult.StatusCode);
    }
}