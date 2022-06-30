using FourKeyMetrics.Entity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FourKeyMetrics.Controllers;

[ApiController]
[Route("[controller]")]
public class FourKeyMetricController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private readonly IMongoCollection<Deployment> _deployments;
    public FourKeyMetricController(ILogger<FourKeyMetricController> logger)
    {
        _logger = logger;
        _deployments = DeploymentDb.Open();
    }

    [HttpGet(Name = "GetFourKeyMetric")]
    public Task<List<Deployment>> Get()
    {
        return _deployments.Find(_ => true).ToListAsync();
    }

    [HttpPost(Name = "CronFourKeyMetric")]
    public void Post(Deployment deployment)
    {
        Console.Write(deployment);
        _deployments.InsertOne(deployment);
    }

}
