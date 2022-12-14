
using devops_metrics.Services.DevOpsMetricServices;

namespace devops_metrics.Models;

public class DeploymentFrequencyModel
{
    public DeploymentFrequencyModel(double dailyMedian, double weeklyMedian, double monthlyMedian, List<DeploymentBucket> deployments)
    {
        DailyMedian = !double.IsNaN(dailyMedian) ? dailyMedian : 0;
        WeeklyMedian = !double.IsNaN(weeklyMedian) ? weeklyMedian : 0;
        MonthlyMedian = !double.IsNaN(monthlyMedian) ? monthlyMedian : 0;
        Deployments = deployments;
    }

    public double DailyMedian { get; set; }
    public double WeeklyMedian { get; set; }
    public double MonthlyMedian { get; set; }
    
    public List<DeploymentBucket> Deployments { get; set; }
}