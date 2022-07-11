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
    public class FailureDeploymentBucket
    {
        public FailureDeploymentBucket(int dayNumber, int weekNumber, int monthNumber, int yearNumber)
        {
            this.YearNumber = yearNumber;
            this.MonthNumber = monthNumber;
            this.WeekNumber = weekNumber;
            this.DayNumber = dayNumber;
            this.DeploymentsInBucket = new List<FailureDeployment>();
        }

        public int DayNumber { get; set; }
        public int WeekNumber { get; set; }
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
        public List<FailureDeployment> DeploymentsInBucket { get; set; }
        public double GetDeploymentDays() => this.DeploymentsInBucket.Count();
    }
    private List<FailureDeploymentBucket> GetDeploymentBuckets(List<FailureDeployment> deployments)
    {
        List<FailureDeploymentBucket> deploymentBucket = new List<FailureDeploymentBucket>();
        
        deployments.ForEach(deployment =>
        {
            var startDate = deployment.deployment.GetStartDateTime();
            var weekNr = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(startDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var bucket = deploymentBucket.Find(iB =>
                iB.WeekNumber == deployment.deployment.GetWeek() && iB.DayNumber == startDate.Day &&
                iB.MonthNumber == startDate.Month && iB.YearNumber == startDate.Year);
            if (bucket == null)
            {
                bucket = new FailureDeploymentBucket(startDate.Day, weekNr, startDate.Month, startDate.Year);
                deploymentBucket.Add(bucket);
            }
            bucket.DeploymentsInBucket.Add(deployment);
        });
        return deploymentBucket.OrderBy(bucket => bucket.YearNumber).ThenBy(bucket => bucket.MonthNumber)
            .ThenBy(bucket => bucket.DayNumber).ToList();
    }

    public class FailureDeployment
    {
        public bool IsFailure = false;
        public Deployment deployment;

        public FailureDeployment(Deployment deployment)
        {
            this.deployment = deployment;
        }
    }
    
    public async Task<ChangeFailureRateModel> Calculate(int intervalMonths, string? organization, string? project, string? repository)
    {
        var incidents = await _incidentService.GetIncidents(intervalMonths, organization, project, repository);
        var incidentsList = await incidents.ToListAsync();
        var deployments = await _deploymentService.GetDeployments(intervalMonths, organization, project, repository);
        var deploymentList = await deployments.ToListAsync();
        var deploymentsWithFailure = deploymentList.Select(deployment => new FailureDeployment(deployment)).ToList();
    
        incidentsList.ForEach(incident =>
        {

            var deployment = deploymentsWithFailure
                .Where(deployment => deployment.deployment.StartTime >= incident.FinishTime 
                                     && deployment.deployment.Organization == incident.Organization
                                     && deployment.deployment.Project == incident.Project
                                     && deployment.deployment.Repository == incident.Repository
                                     && deployment.deployment.Platform == incident.Platform
                                     )
                .MinBy(deployment => deployment.deployment.StartTime);
            if (deployment != null)
            {
                deployment.IsFailure = true;
            }
        });

        var deploymentBuckets = GetDeploymentBuckets(deploymentsWithFailure);

        double failCount =  deploymentsWithFailure.FindAll(x => x.IsFailure).Count;
        double totalCount = deploymentsWithFailure.Count;
        
        var cfrDay  = deploymentBuckets.Select(deploymentBucket =>
        {
            var numberOfFailures = deploymentBucket.DeploymentsInBucket.FindAll(deployment => deployment.IsFailure == true)?.Count ?? 0;
            return new cfrDay{
                Key = new DayKey{DayNumber = deploymentBucket.DayNumber, WeekNumber = deploymentBucket.WeekNumber, MonthNumber = deploymentBucket.MonthNumber, YearNumber = deploymentBucket.YearNumber},
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
                    weekBucket.deployments.Select(day => day.FindAll(i => i.IsFailure == true).Count).Sum();
                return new cfrWeek{
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
                    monthBucket.deployments.Select(day => day.FindAll(i => i.IsFailure == true).Count).Sum();

                return new cfrMonth{
                    Key = new MonthKey{MonthNumber = monthBucket.Key.MonthNumber, YearNumber = monthBucket.Key.YearNumber},
                    ChangeFailureRate = numberOfFailures / monthBucket.deployments.Select(day => day.Count).Sum()
                };
            });
         
        
        return new ChangeFailureRateModel(failCount/totalCount, cfrDay, cfrWeek, cfrMonth);
    }
}