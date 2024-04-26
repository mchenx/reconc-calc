using Cargill.Reconc.Business;
using Cargill.Reconc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TradingsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly TradingBusinessLogic _business;

    public TradingsController(ILogger<TradingsController> logger, TradingBusinessLogic business)
    {
        _logger = logger;
        _business = business;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _business.GetAll());
    }

    [HttpGet("{tradingId}")]
    public async Task<IActionResult> GetById(int tradingId)
    {
        var result = await _business.GetById(tradingId);

        if(result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Trading trade)
    {
        if (await _business.AddTrading(trade))
            return Ok();

        return Problem("Unable to create trading record.");
    }
}
