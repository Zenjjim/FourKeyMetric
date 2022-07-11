
using devops_metrics.Models;
using devops_metrics.Services.DevOpsMetricService;
using Microsoft.AspNetCore.Mvc;

namespace devops_metrics.Controllers;

[ApiController]
[Route("[controller]")]

public class RestoreServiceTimeController : ControllerBase
{
    private readonly ILogger<RestoreServiceTimeController> _logger;
    private readonly RestoreServiceTimeService _rstService;
    
    public RestoreServiceTimeController(ILogger<RestoreServiceTimeController> logger)
    {
        _logger = logger;
        _rstService = new RestoreServiceTimeService();
    }

    [HttpGet(Name = "GetRestoreServiceTime")]
    public Task<RestoreServiceTimeModel> Get([FromQuery(Name = "organization")] string? organization,
        [FromQuery(Name = "project")] string? project, [FromQuery(Name = "repository")] string? repository,
        [FromQuery(Name = "intervalMonths")] int intervalMonths = 3)
    {
        return _rstService.Calculate(-Math.Abs(intervalMonths), organization, project, repository);
    }
}