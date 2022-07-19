using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using devops_metrics.Services.DevOpsMetricServices;
using ImTools;
namespace devops_metrics.test;
public class RestoreServiceTimeTest
{
    private readonly RestoreServiceTimeService _rstService;
    private readonly DateTime _now;
    private readonly RestoreServiceTimeModel _bucket;
    public RestoreServiceTimeTest()
    {
        _rstService = new RestoreServiceTimeService();
        _now = DateTime.Now;
        _bucket = _rstService.CalculateBuckets(new List<Incident>
        {
            Factory.IncidentFactory(startDate: _now.AddHours(-1), finishDate: _now),
        });
    }

    [Fact]
    public void TestCheckCorrectReturnData()
    {
        Assert.True(_bucket.MedianRestoreServiceTime is double);
        Assert.True(_bucket.WeeklyRestoreServiceTime is IEnumerable<Weekly>);
        Assert.True(_bucket.MonthlyRestoreServiceTime is IEnumerable<Monthly>);
        Assert.True(_bucket.Incidents is List<Incident>);
    }
    
    [Theory]
    [InlineData(9, 1, 3600*1)]
    [InlineData(9, 2, 3600*2)]
    [InlineData(9, 3, 3600*2)]    
    [InlineData(13, 1, 3600*1)]
    [InlineData(13, 2, (3600 / 2) + (3600 * 1))]
    [InlineData(13, 3, (3600 / 2) + (3600 * 2))]
    [InlineData(13, 4, (3600 / 2) + (3600 * 3))]
    public void TestRestorationTimeHours(int start, int hour, int expected)
    {
        var now = DateTime.Today + new TimeSpan(start, 0, 0);
        var bucket = _rstService.CalculateBuckets(new List<Incident>
        {
            Factory.IncidentFactory(startDate: now.AddHours(-hour), finishDate: now),
        });
        Assert.Equal(expected, bucket.MedianRestoreServiceTime);
    }

    [Fact]
    public void TestRestorationWeeklyList()
    {
        _bucket.WeeklyRestoreServiceTime.ToList().ForEach(bucket =>
        {
            if (bucket.Key.WeekNumber ==
                DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(_now, CalendarWeekRule.FirstFullWeek,
                    DayOfWeek.Monday) && bucket.Key.YearNumber == _now.Year)
            {
                Assert.Equal(3600, bucket.Median);
            } else
            {
                Assert.True(false);   
            }
        });
    }
}