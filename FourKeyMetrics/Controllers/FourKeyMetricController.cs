using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using FourKeyMetrics.Entities;
using FourKeyMetrics.Service;
using FourKeyMetrics.Service.FourKeyService;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FourKeyMetrics.Controllers;

[ApiController]
[Route("[controller]")]
public class FourKeyMetricController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private readonly FetchDataService _fetchDataService;
    private readonly DeploymentFrequencyService _dfService;
    public FourKeyMetricController(ILogger<FourKeyMetricController> logger)
    {
        _logger = logger;
        _fetchDataService = new FetchDataService();
        _dfService = new DeploymentFrequencyService();
    }

    [HttpGet(Name = "GetFourKeyMetric")]
    public void Get()
    {
        _dfService.Calculate();
    }

    [HttpPost(Name = "CronFourKeyMetric")]
    public void Post()
    {
        _fetchDataService.FetchAllData();
        Ok();

    }

}
