using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using MathNet.Numerics.Statistics;
using MongoDB.Driver;

namespace devops_metrics.Services.DevOpsMetricServices;

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
        var changesList = await new ChangeService().GetChanges(intervalMonths, organization, project, repository);
        return CalculateBuckets(intervalMonths, changesList);
    }

    public LeadTimeChangeModel CalculateBuckets(int intervalMonths, List<Change> changesList)
    {
        var changesBucket = GetBuckets(intervalMonths, changesList);
        var total = changesBucket.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Where(b => b != 0)
            .Median();
        total = double.IsNaN(total) ? 0 : total;
        var weekly = changesBucket.GroupBy(bucket => new { bucket.WeekNumber, bucket.YearNumber})
            .Select(week =>
            {
                var med = week.Select(day => day.GetLeadChangeTime()).SelectMany(day => day)
                    .Where(b => b != 0).Median();
                return new Weekly
                {
                    Key = new WeekKey
                    {
                        WeekNumber = week.Key.WeekNumber,
                        YearNumber = week.Key.YearNumber
                    },
                    Median =
                        double.IsNaN(med)
                            ? null
                            : med
                };
            });        
        var monthly = changesBucket.GroupBy(bucket => new { bucket.MonthNumber, bucket.YearNumber })
            .Select(month =>
            {
                var med = month.Select(day => day.GetLeadChangeTime()).SelectMany(day => day).Where(b => b != 0)
                    .Median();
                return new Monthly
                {
                    Key = new MonthKey { MonthNumber = month.Key.MonthNumber, YearNumber = month.Key.YearNumber },
                    Median = double.IsNaN(med)
                        ? null
                        : med
                };
            

            });
        
        return new LeadTimeChangeModel(total, weekly, monthly, changesList.OrderBy(change => change.StartTime));
    }
}