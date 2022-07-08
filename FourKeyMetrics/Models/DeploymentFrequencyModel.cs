using FourKeyMetrics.Services.FourKeyService;

namespace FourKeyMetrics.Models;

public class DeploymentFrequencyModel
{
    public DeploymentFrequencyModel(double dailyMedian, double weeklyMedian, double monthlyMedian, List<DeploymentBucket> weeklyDeployments)
    {
        DailyMedian = !double.IsNaN(dailyMedian) ? weeklyMedian : 0;
        WeeklyMedian = !double.IsNaN(weeklyMedian) ? weeklyMedian : 0;
        MonthlyMedian = !double.IsNaN(monthlyMedian) ? monthlyMedian : 0;
        WeeklyDeployments = weeklyDeployments;
    }

    public double DailyMedian { get; set; }
    public double WeeklyMedian { get; set; }
    public double MonthlyMedian { get; set; }
    public List<DeploymentBucket> WeeklyDeployments { get; set; }
}