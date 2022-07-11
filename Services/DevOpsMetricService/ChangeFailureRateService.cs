using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using MathNet.Numerics.Statistics;
using MongoDB.Driver;

namespace devops_metrics.Services.DevOpsMetricService;

public class ChangeFailureRateService
{
    private readonly DeploymentService _deploymentService;
    private readonly IncidentService _incidentService;

    public ChangeFailureRateService()
    {
        _deploymentService = new DeploymentService();
        _incidentService = new IncidentService();
    }
    
    private List<DeploymentBucket> GetDeploymentBuckets(List<Deployment> deployments)
    {
        List<DeploymentBucket> deploymentBucket = new List<DeploymentBucket>();

        deployments.ForEach(deployment =>
        {
            var startDate = deployment.GetStartDateTime();
            var weekNr = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(startDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var bucket = deploymentBucket.Find(iB =>
                iB.WeekNumber == deployment.GetWeek() && iB.DayNumber == startDate.Day &&
                iB.MonthNumber == startDate.Month && iB.YearNumber == startDate.Year);
            if (bucket == null)
            {
                bucket = new DeploymentBucket(startDate.Day, weekNr, startDate.Month, startDate.Year);
                deploymentBucket.Add(bucket);
            }
            bucket.DeploymentsInBucket.Add(deployment);
        });
        return deploymentBucket.OrderBy(bucket => bucket.YearNumber).ThenBy(bucket => bucket.MonthNumber)
            .ThenBy(bucket => bucket.DayNumber).ToList();
    }
    private List<IncidentBucket> getIncidentBuckets(List<Incident> incidents)
    {
        
        List<IncidentBucket> incidentBucket = new List<IncidentBucket>();

        incidents.ForEach(incident =>
        {
            var finishDate = incident.GetFinishDateTime();
            var weekNr = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(finishDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var bucket = incidentBucket.Find(iB =>
                iB.WeekNumber == incident.GetWeek(false) && iB.DayNumber == finishDate.Day &&
                iB.MonthNumber == finishDate.Month && iB.YearNumber == finishDate.Year);
            if (bucket == null)
            {
                bucket = new IncidentBucket(finishDate.Day, weekNr, finishDate.Month, finishDate.Year);
                incidentBucket.Add(bucket);
            }
            bucket.IncidentsInBucket.Add(incident);
        });
        return incidentBucket.OrderBy(bucket => bucket.YearNumber).ThenBy(bucket => bucket.MonthNumber)
            .ThenBy(bucket => bucket.DayNumber).ToList();
    }
    
    public async Task<ChangeFailureRateModel> Calculate(int intervalMonths, string? organization, string? project, string? repository)
    {
        var incidents = await _incidentService.GetIncidents(intervalMonths, organization, project, repository);
        var incidentsList = await incidents.ToListAsync();
        
        var deployments = await _deploymentService.GetDeployments(intervalMonths, organization, project, repository);
        var deploymentList = await deployments.ToListAsync();

        incidentsList.ForEach(incident =>
        {
            deploymentList
                .Where(deployment => deployment.StartTime >= incident.FinishTime)
                .OrderBy(deployment => deployment.StartTime).First().isFailure = true;
            
        });

        var deploymentBuckets = GetDeploymentBuckets(deploymentList);
        
        
        var cfrDay  = deploymentBuckets.Select(deploymentBucket =>
        {
            var numberOfFailures = deploymentBucket.DeploymentsInBucket.FindAll(deployment => deployment.isFailure == true)?.Count ?? 0;
            return new cfrDay {
                DayNumber = deploymentBucket.DayNumber,
                WeekNumber = deploymentBucket.WeekNumber,
                MonthNumber = deploymentBucket.MonthNumber,
                YearNumber = deploymentBucket.YearNumber,
                ChangeFailureRate = numberOfFailures / deploymentBucket.GetDeploymentDays()
            };
        });
        var cfrWeek = deploymentBuckets.GroupBy(bucket => new { bucket.WeekNumber, bucket.MonthNumber, bucket.YearNumber })
            .Select(week => new
            {
                week.Key,
                deployments = week.Select(day => day.DeploymentsInBucket)
            }).Select(weekBucket =>
            {
                double numberOfFailures =
                    weekBucket.deployments.Select(day => day.FindAll(i => i.isFailure == true).Count).Sum();
                return new cfrWeek
                {
                    Key = new WeekKey{WeekNumber = weekBucket.Key.WeekNumber, MonthNumber = weekBucket.Key.MonthNumber, YearNumber = weekBucket.Key.YearNumber},
                    ChangeFailureRate = numberOfFailures / weekBucket.deployments.Select(day => day.Count).Sum()
                };
            });
        var cfrMonth = deploymentBuckets.GroupBy(bucket => new { bucket.MonthNumber, bucket.YearNumber })
            .Select(month => new
            {
                month.Key,
                deployments = month.Select(day => day.DeploymentsInBucket)
            }).Select(monthBucket =>
            {
                double numberOfFailures =
                    monthBucket.deployments.Select(day => day.FindAll(i => i.isFailure == true).Count).Sum();

                return new cfrMonth
                {
                    Key = new MonthKey{MonthNumber = monthBucket.Key.MonthNumber, YearNumber = monthBucket.Key.YearNumber},
                    ChangeFailureRate = numberOfFailures / monthBucket.deployments.Select(day => day.Count).Sum()
                };
            });
         
        
        return new ChangeFailureRateModel(cfrDay, cfrWeek, cfrMonth);
    }
}