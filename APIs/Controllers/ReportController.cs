using Cargill.Reconc.Business;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ReconcCalculator _calculator;
    private readonly ITradingBusinessLogic _tradingBusiness;
    private readonly IInsuranceBusinessLogic _insuranceBusiness;
    private readonly ICounterpartyBusinessLogic _counterpartiesBusiness;

    public ReportsController(
        ILogger<ReportsController> logger, 
        ReconcCalculator calculator,
        ITradingBusinessLogic tradingBusiness,
        IInsuranceBusinessLogic insuranceBusiness,
        ICounterpartyBusinessLogic counterpartiesBusiness)
    {
        _logger = logger;
        _calculator = calculator;
        _tradingBusiness = tradingBusiness;
        _insuranceBusiness = insuranceBusiness;
        _counterpartiesBusiness = counterpartiesBusiness;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tradingsTask = _tradingBusiness.GetAll();
        var insuranceTask = _insuranceBusiness.GetAll();
        var counterpartiesTask = _counterpartiesBusiness.GetAll();

        await Task.WhenAll(tradingsTask, insuranceTask, counterpartiesTask);

        return Ok(await _calculator.GetBatchReports(tradingsTask.Result, counterpartiesTask.Result, insuranceTask.Result));
    }

    [HttpGet("{tradingId}")]
    public async Task<IActionResult> GetByTradingId(int tradingId)
    {
        var insuranceTask = _insuranceBusiness.GetById(tradingId);
        var tradingAggTask = _tradingBusiness.AggregateBySupplier(tradingId);
        
        await Task.WhenAll(tradingAggTask, tradingAggTask);

        if(tradingAggTask.Result == null)
            return NotFound();

        var pdRate = await _counterpartiesBusiness.GetPDRate(tradingAggTask.Result.SupplierCode);

        return Ok(_calculator.GetReportByTrading(tradingAggTask.Result, insuranceTask.Result, pdRate??0));
    }
}
