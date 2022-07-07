using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using FourKeyMetrics.Entities;
using FourKeyMetrics.Models;
using FourKeyMetrics.Service;
using FourKeyMetrics.Services.FourKeyService;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FourKeyMetrics.Controllers;

[ApiController]
[Route("[controller]")]

public class DeploymentFrequencyController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private readonly DeploymentFrequencyService _dfService;
    
    public DeploymentFrequencyController(ILogger<FourKeyMetricController> logger)
    {
        _logger = logger;
        _dfService = new DeploymentFrequencyService();
    }

    [HttpGet(Name = "GetDeploymentFrequency")]
    public Task<DeploymentFrequencyModel> Get([FromQuery(Name = "organization")] string? organization,
        [FromQuery(Name = "project")] string? project, [FromQuery(Name = "repository")] string? repository,
        [FromQuery(Name = "intervalMonths")] int intervalMonths = 3)
    {
        return _dfService.Calculate(-Math.Abs(intervalMonths), organization, project, repository);
    }
}
