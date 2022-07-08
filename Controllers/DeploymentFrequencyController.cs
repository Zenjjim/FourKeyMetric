using devops_metrics.Models;
using devops_metrics.Services.DevOpsMetricService;
using Microsoft.AspNetCore.Mvc;

namespace devops_metrics.Controllers;

[ApiController]
[Route("[controller]")]

public class DeploymentFrequencyController : ControllerBase
{
    private readonly ILogger<DeploymentFrequencyController> _logger;
    private readonly DeploymentFrequencyService _dfService;
    
    public DeploymentFrequencyController(ILogger<DeploymentFrequencyController> logger)
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
