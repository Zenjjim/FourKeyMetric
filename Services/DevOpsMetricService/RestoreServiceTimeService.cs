using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using MathNet.Numerics.Statistics;
using MongoDB.Driver;

namespace devops_metrics.Services.DevOpsMetricService;

public class IncidentBucket
{
    public IncidentBucket(int dayNumber, int weekNumber, int monthNumber, int yearNumber)
    {
        this.YearNumber = yearNumber;
        this.MonthNumber = monthNumber;
        this.WeekNumber = weekNumber;
        this.DayNumber = dayNumber;
        this.IncidentsInBucket = new List<Incident>();
    }

    public int DayNumber { get; set; }
    public int WeekNumber { get; set; }
    public int MonthNumber { get; set; }
    public int YearNumber { get; set; }
    public List<Incident> IncidentsInBucket { get; set; }
    public IEnumerable<double> GetLeadChangeTime() => this.IncidentsInBucket.Select(IB => IB.Delta());

}

public class RestoreServiceTimeService
{
    private readonly DeploymentService _deploymentService;
    private readonly IncidentService _incidentService;

    public RestoreServiceTimeService()
    {
        _deploymentService = new DeploymentService();
        _incidentService = new IncidentService();
    }

    private List<IncidentBucket> GetBuckets(List<Incident> incidents)
    {
        
        List<IncidentBucket> incidentBucket = new List<IncidentBucket>();

        incidents.ForEach(incident =>
        {
            var startDate = incident.GetStartDateTime();
            var weekNr = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(startDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var bucket = incidentBucket.Find(iB =>
                iB.WeekNumber == incident.GetWeek() && iB.DayNumber == startDate.Day &&
                iB.MonthNumber == startDate.Month && iB.YearNumber == startDate.Year);
            if (bucket == null)
            {
                bucket = new IncidentBucket(startDate.Day, weekNr, startDate.Month, startDate.Year);
                incidentBucket.Add(bucket);
            }
            bucket.IncidentsInBucket.Add(incident);
        });
        return incidentBucket.OrderBy(bucket => bucket.YearNumber).ThenBy(bucket => bucket.MonthNumber)
            .ThenBy(bucket => bucket.DayNumber).ToList();
    }

    public async Task<RestoreServiceTimeModel> Calculate(int intervalMonths, string? organization, string? project, string? repository)
    {
        var incidents = await _incidentService.GetIncidents(intervalMonths, organization, project, repository);
        var incidentsList = await incidents.ToListAsync();
        var incidentsBucket = GetBuckets(incidentsList);
        var total = incidentsBucket.Select(i => i.GetLeadChangeTime()).SelectMany(a => a).Median();
        var weekly = incidentsBucket.GroupBy(bucket => new { bucket.WeekNumber, bucket.YearNumber, bucket.MonthNumber })
            .Select(week => new {
                week.Key,
                Median = week.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Median()
            });        
        var monthly = incidentsBucket.GroupBy(bucket => new { bucket.MonthNumber, bucket.YearNumber })
            .Select(week => new {
                week.Key,
                Median = week.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Median()
            });
        
        return new RestoreServiceTimeModel(total, weekly, monthly, incidentsList);
    }
}