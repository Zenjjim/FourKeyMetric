

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
    public DeploymentService()
    {
        _deployments = DeploymentDb.Open();
    }


    public void InsertAllDeploymentData(List<Deployment> deployments)
    {

        try
        {
            _deployments.InsertMany(deployments, new InsertManyOptions
            {
                IsOrdered = false
            });
        }
        catch (MongoBulkWriteException){}
    } 
}

