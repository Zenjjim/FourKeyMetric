using System.Globalization;
using devops_metrics.Entities;
using devops_metrics.Models;
using devops_metrics.Services.DevOpsMetricServices;
using ImTools;

namespace devops_metrics.test;
public class ChangeFailureRateTest
{
    private readonly ChangeFailureRateService _cfrService;
    private readonly DateTime _now;
    private readonly ChangeFailureRateModel _bucket;
    public ChangeFailureRateTest()
    {
        _cfrService = new ChangeFailureRateService();
        _now = DateTime.Now;
        _bucket = _cfrService.CalculateBuckets(
            new List<Deployment>
            {
                Factory.DeploymentFactory(startDate: _now, finishDate: _now),
            },
            new List<Incident>
            {
                Factory.IncidentFactory(startDate: _now, finishDate: _now),
            }
        );
    }

    [Fact]
    public void TestCheckCorrectReturnData()
    {
        Assert.True(_bucket.ChangeFailureRate is double);
        Assert.True(_bucket.ChangeFailureRateByDay is IEnumerable<ChangeFailureRateDay>);
        Assert.True(_bucket.ChangeFailureRateByWeek is IEnumerable<ChangeFailureRateWeek>);
        Assert.True(_bucket.ChangeFailureRateByMonth is IEnumerable<ChangeFailureRateMonth>);
        
    }

    [Theory]
    [InlineData(1, 1, (1.0))]
    [InlineData(1, 2, (1.0/2.0))]
    [InlineData(1, 3, (1.0/3.0))]
    [InlineData(1, 4, (1.0/4.0))]
    [InlineData(1, 5, (1.0/5.0))]
    [InlineData(2, 1, (1.0))]
    [InlineData(3, 1, (1.0))]
    [InlineData(4, 1, (1.0))]
    [InlineData(5, 1, (1.0))]
    public void TestFailureRate(int nrOfIncident, int nrOfDeployments, double excepted)
    {
        var now = DateTime.Now;
        var deployments = Enumerable.Range(0, nrOfDeployments)
            .Map(i => Factory.DeploymentFactory(startDate: now.AddDays(-i), finishDate: now.AddDays(-i).AddMinutes(1))).ToList();
        var incidents = Enumerable.Range(0, nrOfIncident)
            .Map(i => Factory.IncidentFactory(startDate: now.AddDays(-nrOfDeployments), finishDate: now.AddDays(-nrOfDeployments).AddMinutes(1))).ToList();
        var bucket = _cfrService.CalculateBuckets(deployments, incidents);
        Assert.Equal(nrOfDeployments, deployments.Count);
        Assert.Equal(nrOfIncident, incidents.Count);
        Assert.Equal(excepted, bucket.ChangeFailureRate);
    }

    [Fact]
    public void TestFailureRatePerWeek()
    {
        bool found = false;
        _bucket.ChangeFailureRateByWeek.ToList().ForEach(cfr =>
        {
            if (cfr.Key.YearNumber == _now.Year && cfr.Key.MonthNumber == _now.Month && cfr.Key.WeekNumber ==
                DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(_now, CalendarWeekRule.FirstFullWeek,
                    DayOfWeek.Monday))
            {
                Assert.Equal(1, cfr.ChangeFailureRate);
                found = true;
            }
            else
            {
                Assert.Equal(0, cfr.ChangeFailureRate);
            }
        });
        Assert.True(found);
    }
    [Fact]
    public void TestFailureRatePerMonth()
    {
        
        bool found = false;
        _bucket.ChangeFailureRateByMonth.ToList().ForEach(cfr =>
        {
            if (cfr.Key.YearNumber == _now.Year && cfr.Key.MonthNumber == _now.Month )
            {
                Assert.Equal(1, cfr.ChangeFailureRate);
                found = true;
            }
            else
            {
                Assert.Equal(0, cfr.ChangeFailureRate);
            }
        });
        Assert.True(found);
    }    
    [Fact]
    public void TestFailureRatePerDay()
    {
        bool found = false;
        _bucket.ChangeFailureRateByDay.ToList().ForEach(cfr =>
        {
            if (cfr.Key.YearNumber == _now.Year && cfr.Key.MonthNumber == _now.Month && cfr.Key.DayNumber == _now.Day )
            {
                Assert.Equal(1, cfr.ChangeFailureRate);
                found = true;
            }
            else
            {
                Assert.Equal(0, cfr.ChangeFailureRate);
            }
        });
        Assert.True(found);
    }
}