using devops_metrics.Models;
using devops_metrics.Services.DevOpsMetricServices;
using Microsoft.AspNetCore.Mvc;

namespace devops_metrics.Controllers;

[ApiController]
[Route("[controller]")]

public class LeadTimeChangeController : ControllerBase
{
    private readonly ILogger<LeadTimeChangeController> _logger;
    private readonly LeadTimeChangeService _ltcService;
    
    public LeadTimeChangeController(ILogger<LeadTimeChangeController> logger)
    {
        _logger = logger;
        _ltcService = new LeadTimeChangeService();
    }

    [HttpGet(Name = "GetLeadTimeChange")]
    public Task<LeadTimeChangeModel> Get([FromQuery(Name = "organization")] string? organization,
        [FromQuery(Name = "project")] string? project, [FromQuery(Name = "repository")] string? repository,
        [FromQuery(Name = "intervalMonths")] int intervalMonths = 3)
    {
        return _ltcService.Calculate(-Math.Abs(intervalMonths), organization, project, repository);
    }
}