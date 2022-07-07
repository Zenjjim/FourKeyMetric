using FourKeyMetrics.Service;
using Microsoft.AspNetCore.Mvc;

namespace FourKeyMetrics.Controllers;

[ApiController]
[Route("[controller]")]

public class FourKeyMetricController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private readonly FetchDataService _fetchDataService;
    public FourKeyMetricController(ILogger<FourKeyMetricController> logger)
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
