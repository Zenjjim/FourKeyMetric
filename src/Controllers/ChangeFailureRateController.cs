
using devops_metrics.Models;
using devops_metrics.Services.DevOpsMetricServices;
using Microsoft.AspNetCore.Mvc;

namespace devops_metrics.Controllers;

[ApiController]
[Route("[controller]")]

public class ChangeFailureRateController : ControllerBase
{
    private readonly ILogger<ChangeFailureRateController> _logger;
    private readonly ChangeFailureRateService _cfrService;
    
    public ChangeFailureRateController(ILogger<ChangeFailureRateController> logger)
    {
        _logger = logger;
        _cfrService = new ChangeFailureRateService();
    }

    [HttpGet(Name = "GetChangeFailureRateService")]
    public Task<ChangeFailureRateModel> Get([FromQuery(Name = "organization")] string? organization,
        [FromQuery(Name = "project")] string? project, [FromQuery(Name = "repository")] string? repository,
        [FromQuery(Name = "intervalMonths")] int intervalMonths = 3)
    {
        return _cfrService.Calculate(-Math.Abs(intervalMonths), organization, project, repository);
    }
}