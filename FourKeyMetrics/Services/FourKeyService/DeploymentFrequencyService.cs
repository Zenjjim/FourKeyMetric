using FourKeyMetrics.Models;
using FourKeyMetrics.Service;
using MathNet.Numerics.Statistics;

namespace FourKeyMetrics.Services.FourKeyService;

public class DeploymentFrequencyService
{
    private readonly DeploymentService _deploymentService;

    public DeploymentFrequencyService()
    {
        _deploymentService = new DeploymentService();
    }

    public async Task<DeploymentFrequencyModel> Calculate(int intervalMonths, string? organization, string? project, string? repository)
    {
        
        var deployments = await _deploymentService.GetDeployments(intervalMonths, organization, project, repository);
        var deploymentWeeks = await Utils.GetWeeks(intervalMonths, deployments);
        return new DeploymentFrequencyModel(deploymentWeeks.Select(dW => dW.GetDeploymentDays()).Median(), deploymentWeeks);
    }

}