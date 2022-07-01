using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using FourKeyMetrics.Entities;
using FourKeyMetrics.Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FourKeyMetrics.Controllers;

[ApiController]
[Route("[controller]")]
public class FourKeyMetricController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private FourKeyMetricService _fourKeyMetricService;
    public FourKeyMetricController(ILogger<FourKeyMetricController> logger)
    {
        _logger = logger;
        _fourKeyMetricService = new FourKeyMetricService();
    }

    [HttpGet(Name = "GetFourKeyMetric")]
    public string Get()
    {
        return _fourKeyMetricService.Get();
    }

    [HttpPost(Name = "CronFourKeyMetric")]
    public void Post()
    {
        _fourKeyMetricService.FetchAllData();
        Ok();

    }

}
