using devops_metrics.Entities;
using devops_metrics.Services.DevOpsMetricServices;

namespace devops_metrics.test;
public class LeadTimeChangeTest
{
    private readonly LeadTimeChangeService _ltcService;
    private readonly double _workday;
    public LeadTimeChangeTest()
    {
        _ltcService = new LeadTimeChangeService();
        var dayStart = new TimeSpan(7, 0, 0);
        var lunchStart = new TimeSpan(11, 0, 0);
        var lunchEnd = new TimeSpan(11, 30, 0);
        var dayEnd = new TimeSpan(17, 0, 0);
        _workday = (lunchStart - dayStart).TotalSeconds + (dayEnd-lunchEnd).TotalSeconds;
    }

    [Fact]
    public void TestCheckCorrectReturnData()
    {
        int interval = -1;
        var bucket = _ltcService.CalculateBuckets(interval, new List<Change>
        {
            Factory.ChangeFactory(startDate:DateTime.Now.AddDays(-4), finishDate:DateTime.Now.AddDays(-1)),
        });
        Assert.True(bucket.MedianLeadTimeChange is double);
        Assert.True(bucket.WeeklyLeadTimeChange is double);
        Assert.True(bucket.MonthlyLeadTimeChange is double);
        Assert.True(bucket.Changes is List<DeploymentBucket>);
        
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void TestMedialLeadTimeChange(int leadTimeDays)
    {
        int interval = -1;
        var now = DateTime.Today;
        int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Friday)) % 7;
        now = now.AddDays(-1 * diff).Date;
        now = now + new TimeSpan(7, 0, 0);
 
        var bucket = _ltcService.CalculateBuckets(interval, new List<Change>
        {
            Factory.ChangeFactory(startDate:now.AddDays(-leadTimeDays), finishDate:now),
        });
        Assert.Equal(leadTimeDays*_workday, bucket.MedianLeadTimeChange);
    }
}