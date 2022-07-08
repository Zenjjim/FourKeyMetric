using System.Globalization;
using FourKeyMetrics.Entities;
using MongoDB.Driver;

namespace FourKeyMetrics.Services.FourKeyService;

public class DeploymentWeek
{
    public DeploymentWeek(int weekNumber)
    {
        this.WeekNumber = weekNumber;
        this.DeploymentsInWeek = new List<Deployment>();
    }

    public int WeekNumber { get; set; }
    public List<Deployment> DeploymentsInWeek { get; set; }

    public double GetDeploymentDays()
    {
        return this.DeploymentsInWeek.Select(dW => dW.GetStartDateTime().Date).Distinct().Count();
    }
}

class Utils {
    public static async Task<List<DeploymentWeek>> GetWeeks(int intervalMonths,  IAsyncCursor<Deployment> deployments)
    {
        var now = DateTime.Now;
        var intervalDate = now.AddMonths(intervalMonths);
        
        var startWeek = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(intervalDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        var endWeek = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        
        List<DeploymentWeek> deploymentWeeks = new List<DeploymentWeek>();
        
        for (int i = startWeek; i < endWeek; i++)
        {
            deploymentWeeks.Add(new DeploymentWeek(i));
        }
        await deployments.ForEachAsync(deployment =>
        {
                deploymentWeeks.Find(dW => dW.WeekNumber == deployment.GetWeek())?.DeploymentsInWeek.Add(deployment);
        });
        return deploymentWeeks;
    }
}

