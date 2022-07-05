using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Service.FourKeyService;

public class DeploymentFrequencyService
{
    private readonly DeploymentService _deploymentService;

    public DeploymentFrequencyService()
    {
        _deploymentService = new DeploymentService();
    }

    public void Calculate()
    {
        _deploymentService.GetDeployments();
    }
}