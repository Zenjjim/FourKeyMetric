using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using MathNet.Numerics.Statistics;
using MongoDB.Driver;

namespace devops_metrics.Services.DevOpsMetricServices;
public class DeploymentBucket
{
    public DeploymentBucket(int dayNumber, int weekNumber, int monthNumber, int yearNumber)
    {
        this.YearNumber = yearNumber;
        this.MonthNumber = monthNumber;
        this.WeekNumber = weekNumber;
        this.DayNumber = dayNumber;
        this.DeploymentsInBucket = new List<Deployment>();
    }

    public int DayNumber { get; set; }
    public int WeekNumber { get; set; }
    public int MonthNumber { get; set; }
    public int YearNumber { get; set; }
    public List<Deployment> DeploymentsInBucket { get; set; }
    public double GetDeploymentDays() => this.DeploymentsInBucket.Count();
}
public class DeploymentFrequencyService
{
    private readonly DeploymentService _deploymentService;

    public DeploymentFrequencyService() => _deploymentService = new DeploymentService();

    public async Task<List<DeploymentBucket>> GetBuckets(int intervalMonths,  IAsyncCursor<Deployment> deployments)
    {
        DateTime intervalDate = DateTime.Now.AddMonths(intervalMonths);
        
        List<DeploymentBucket> deploymentBuckets = new List<DeploymentBucket>();
        while (intervalDate < DateTime.Now)
        {
            var weekNr = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(intervalDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            if (intervalDate.DayOfWeek != DayOfWeek.Saturday && intervalDate.DayOfWeek != DayOfWeek.Sunday)
            {
                deploymentBuckets.Add(new DeploymentBucket(intervalDate.Day, weekNr, intervalDate.Month, intervalDate.Year));
                
            }
            intervalDate = intervalDate.AddDays(1);
        }
        await deployments.ForEachAsync(deployment =>
        {
            deploymentBuckets.Find(dW => dW.WeekNumber == deployment.GetWeek() && dW.DayNumber == deployment.GetStartDateTime().Day && dW.MonthNumber == deployment.GetStartDateTime().Month && dW.YearNumber == deployment.GetStartDateTime().Year)?.DeploymentsInBucket.Add(deployment);
        });
        return deploymentBuckets;
    }

    public async Task<DeploymentFrequencyModel> Calculate(int intervalMonths, string? organization, string? project, string? repository)
    {
        
        var deployments = await _deploymentService.GetDeployments(intervalMonths, organization, project, repository);
        var deploymentBucket = await GetBuckets(intervalMonths, deployments);
        var daily = deploymentBucket.GroupBy(bucket => new { bucket.DayNumber, bucket.MonthNumber, bucket.YearNumber })
            .Select(group => group.Select(item => item.GetDeploymentDays()).Sum()).Median();
        var weekly = deploymentBucket.GroupBy(bucket => new { bucket.WeekNumber, bucket.YearNumber })
            .Select(group => group.Select(item => item.GetDeploymentDays()).Sum()).Median();
        var monthly = deploymentBucket.GroupBy(bucket => new { bucket.MonthNumber, bucket.YearNumber })
            .Select(group => group.Select(item => item.GetDeploymentDays()).Sum()).Median();
        return new DeploymentFrequencyModel(daily, weekly, monthly,  deploymentBucket);
    }

}