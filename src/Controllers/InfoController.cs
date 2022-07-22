using devops_metrics.Models;
using devops_metrics.Services;
using Microsoft.AspNetCore.Mvc;

namespace devops_metrics.Controllers;
[ApiController]
[Route("[controller]")]
public class InfoController : ControllerBase
{
    private readonly ILogger<InfoController> _logger;
    private readonly InfoService _infoService;
    
    public InfoController(ILogger<InfoController> logger)
    {
        _logger = logger;
        _infoService = new InfoService();
    }

    [HttpGet(Name = "GetInfo")]
    public async Task<Dictionary<string, Dictionary<string, HashSet<string>>>> Get()
    {
        return await _infoService.GetInfo();
    }
}