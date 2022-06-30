using FourKeyMetrics.Entity;
using FourKeyMetrics.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FourKeyMetrics.Services;

public class FourKeyMetricService
{
    private readonly IMongoCollection<Deployment> _deployment;        

    public FourKeyMetricService() {
        _deployment = db.GetCollection<Deployment>(settings.DeploymentCollectionName);
    }

    public Deployment GetAll()
    {
        return _deployment.FindASync(_ => true).ToEnumerable();
    }

    public void Create(Deployment deployment)
    {
        _deployment.InsertOneAsync(deployment);
    }

}
