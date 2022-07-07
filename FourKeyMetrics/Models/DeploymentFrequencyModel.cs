using FourKeyMetrics.Services.FourKeyService;

namespace FourKeyMetrics.Models;

public class DeploymentFrequencyModel
{
    public DeploymentFrequencyModel(double median, List<DeploymentWeek> weeklyDeployments)
    {
        Median = median;
        WeeklyDeployments = weeklyDeployments;
    }

    public double Median { get; set; }
    public List<DeploymentWeek> WeeklyDeployments { get; set; }
}