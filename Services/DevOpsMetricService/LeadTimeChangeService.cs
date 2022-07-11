using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using MathNet.Numerics.Statistics;
using MongoDB.Driver;

namespace devops_metrics.Services.DevOpsMetricService;

public class ChangeBucket
{
    public ChangeBucket(int dayNumber, int weekNumber, int monthNumber, int yearNumber)
    {
        this.YearNumber = yearNumber;
        this.MonthNumber = monthNumber;
        this.WeekNumber = weekNumber;
        this.DayNumber = dayNumber;
        this.ChangesInBucket = new List<Change>();
    }

    public int DayNumber { get; set; }
    public int WeekNumber { get; set; }
    public int MonthNumber { get; set; }
    public int YearNumber { get; set; }
    public List<Change> ChangesInBucket { get; set; }
    public IEnumerable<double> GetLeadChangeTime() => this.ChangesInBucket.Select(CB => CB.Delta());
}

public class LeadTimeChangeService
{
    private readonly DeploymentService _deploymentService;
    private readonly ChangeService _changeService;

    public LeadTimeChangeService()
    {
        _deploymentService = new DeploymentService();
        _changeService = new ChangeService();
    }
    
    private List<ChangeBucket> GetBuckets(int intervalMonths, List<Change> changes)
    {
        
        List<ChangeBucket> changeBucket = new List<ChangeBucket>();

        changes.ForEach(change =>
        {
            var startDate = change.GetStartDateTime();
            var weekNr = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(startDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var bucket = changeBucket.Find(cB =>
                cB.WeekNumber == change.GetWeek() && cB.DayNumber == startDate.Day &&
                cB.MonthNumber == startDate.Month && cB.YearNumber == startDate.Year);
            if (bucket == null)
            {
                bucket = new ChangeBucket(startDate.Day, weekNr, startDate.Month, startDate.Year);
                changeBucket.Add(bucket);
            }
            bucket.ChangesInBucket.Add(change);
        });
        return changeBucket.OrderBy(bucket => bucket.YearNumber).ThenBy(bucket => bucket.MonthNumber)
            .ThenBy(bucket => bucket.DayNumber).ToList();
    }

    public async Task<LeadTimeChangeModel> Calculate(int intervalMonths, string? organization, string? project, string? repository)
    {
        var changes = await _changeService.GetChanges(intervalMonths, organization, project, repository);
        var changesList = await changes.ToListAsync();
        var changesBucket = GetBuckets(intervalMonths, changesList);
        var total = changesBucket.Select(i => i.GetLeadChangeTime()).SelectMany(a => a).Median();
        var weekly = changesBucket.GroupBy(bucket => new { bucket.WeekNumber, bucket.YearNumber, bucket.MonthNumber})
            .Select(week => new {
                week.Key,
                Median = week.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Median()
            });        
        var monthly = changesBucket.GroupBy(bucket => new { bucket.MonthNumber, bucket.YearNumber })
            .Select(week => new {
                week.Key,
                Median = week.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Median()
            });
        
        return new LeadTimeChangeModel(total, weekly, monthly, changesList);
    }
}