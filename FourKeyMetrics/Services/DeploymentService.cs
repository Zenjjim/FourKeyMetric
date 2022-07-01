

using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Service;

public class DeploymentService
{
    private readonly IMongoCollection<Deployment> _deployments;
    private AzureManager _azure;
    public DeploymentService()
    {
        _deployments = DeploymentDb.Open();
        _azure = new AzureManager(Environment.GetEnvironmentVariable("AZURE_TOKEN"));
    }

    public Task<List<Deployment>> GetDeployments()
    {
        return _deployments.Find(_ => true).ToListAsync();
    }

    public Task<AzureBuildModel> CreateDeployment()
    {
        return _azure.GetBuilds("mollerdigital","carcare", "82", "refs/heads/main");
        // _deployments.InsertOne(deployment);
    }

    public void InsertAllBuildData()
    {
        String platform = "Azure";
        String organizaion = "mollerdigital";
        var projects = _azure.GetProjects(organizaion).Result.Value;

        var deployments = new List<Deployment>();
        foreach (var project in projects)
        {
            var repositories = _azure.GetRepositories(organizaion, project.Name).Result.Value;
            var definitions = _azure.GetDefinitions(organizaion, project.Name).Result.Value;
            foreach (var definition in definitions)
            {
                var repository = repositories.Find(rep => rep.Name == definition.Name);
                if (repository == null) continue;
                var builds = _azure.GetBuilds(organizaion, project.Name, definition.Id.ToString(), repository.DefaultBranch).Result.Value;
                foreach (var build in builds)
                {
                    String result = build.Result == "succeeded" ? "SUCCESS" : "FAIL";
                    deployments.Add(new Deployment(build.QueueTime.Value.ToUnixTimeSeconds(), build.FinishTime.Value.ToUnixTimeSeconds(), result, repository.Name, definition.Id.ToString(), project.Name, organizaion, build.RequestedFor.DisplayName, platform));
                    
                }
                
            }

        }

        try
        {
            _deployments.InsertMany(deployments, new InsertManyOptions
            {
                IsOrdered = false
            });
        }
        catch (MongoBulkWriteException e)
        {
            Console.WriteLine(e);
        }
    } 
}

