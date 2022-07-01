using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FourKeyMetrics.Service;

public class FourKeyMetricService
{
    private DeploymentService _deploymentService;

    public FourKeyMetricService()
    {
        _deploymentService = new DeploymentService();

        
    }

    public String Get()
    {
        return "Her kommer masse data";
    }

    public void FetchAllData()
    {
        List<Config> configs;
        using (StreamReader r = new StreamReader("config.json"))
        {
            string json = r.ReadToEnd();
            configs = JsonConvert.DeserializeObject<List<Config>>(json);
        }

        foreach (var config in configs)
        {
            _deploymentService.InsertAllBuildData(config.Platform, config.Organization);
        }
    }

}