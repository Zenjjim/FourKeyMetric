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

public class LeadTimeChangeController : ControllerBase
{
    private readonly ILogger<FourKeyMetricController> _logger;
    private readonly LeadTimeChangeService _ltcService;
    
    public LeadTimeChangeController(ILogger<FourKeyMetricController> logger)
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