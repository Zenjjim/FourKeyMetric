using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using FourKeyMetrics.Entities;
using FourKeyMetrics.Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FourKeyMetrics.Controllers;

[ApiController]
[Route("[controller]")]
public class FourKeyMetricController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private DeploymentService _deploymentService;
    public FourKeyMetricController(ILogger<FourKeyMetricController> logger)
    {
        _logger = logger;
        _deploymentService = new DeploymentService();
    }

    [HttpGet(Name = "GetFourKeyMetric")]
    public Task<List<Deployment>> Get()
    {
        return _deploymentService.GetDeployments();
    }

    [HttpPost(Name = "CronFourKeyMetric")]
    public void Post()
    {
        _deploymentService.InsertAllBuildData();
        Ok();

    }

}
