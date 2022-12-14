using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using MathNet.Numerics.Statistics;
using MongoDB.Driver;

namespace devops_metrics.Services.DevOpsMetricServices;

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
        var incidentsList = await new IncidentService().GetIncidents(intervalMonths, organization, project, repository);
        return CalculateBuckets(incidentsList);
    }

    public RestoreServiceTimeModel CalculateBuckets(List<Incident> incidentsList)
    {
        var incidentsBucket = GetBuckets(incidentsList);
        var total = incidentsBucket.Select(i => i.GetLeadChangeTime()).SelectMany(a => a).Where(b => b != 0).Median();
        total = double.IsNaN(total) ? 0 : total;
        var weekly = incidentsBucket.GroupBy(bucket => new { bucket.WeekNumber, bucket.YearNumber, bucket.MonthNumber })
            .Select(week =>
            {
                var med = week.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Where(b => b != 0)
                    .Median();
                return new Weekly
                {
                    Key = new WeekKey
                    {
                        WeekNumber = week.Key.WeekNumber, MonthNumber = week.Key.MonthNumber,
                        YearNumber = week.Key.YearNumber
                    },
                    Median = double.IsNaN(med) ? null : med
                };
            });        
        var monthly = incidentsBucket.GroupBy(bucket => new { bucket.MonthNumber, bucket.YearNumber })
            .Select(month =>
            {
                var med = month.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Where(b => b != 0)
                    .Median();
                return new Monthly
                {
                    Key = new MonthKey { MonthNumber = month.Key.MonthNumber, YearNumber = month.Key.YearNumber },
                    Median = double.IsNaN(med) ? null : med
                };
            });
        
        return new RestoreServiceTimeModel(total, weekly, monthly, incidentsList);
    }
}