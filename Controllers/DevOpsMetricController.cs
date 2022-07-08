using devops_metrics.Services;
using Microsoft.AspNetCore.Mvc;

namespace devops_metrics.Controllers;

[ApiController]
[Route("[controller]")]
public class DevOpsMetricController : ControllerBase
{
    private readonly FetchDataService _fetchDataService;
    private readonly ILogger<DevOpsMetricController> _logger;

    public DevOpsMetricController(ILogger<DevOpsMetricController> logger)
    {
        _logger = logger;
        _fetchDataService = new FetchDataService();
    }


    [HttpPost(Name = "CronFetchData")]
    public void Post()
    {
        _fetchDataService.FetchAllData();
    }
}