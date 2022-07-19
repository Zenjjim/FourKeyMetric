using devops_metrics.Entities;
using devops_metrics.Services.DevOpsMetricServices;

namespace devops_metrics.test;
public class DeploymentFrequencyTest
{
    private DeploymentFrequencyService dfService;
    public DeploymentFrequencyTest()
    {
        dfService = new DeploymentFrequencyService();
    }

    [Fact]
    public void TestCheckCorrectBucketTypes()
    {
        int interval = -1;
        var bucket = dfService.CalculateBuckets(interval, new List<Deployment>
        {
            Factory.DeploymentFactory(startDate:DateTime.Now.AddDays(-4), finishDate:DateTime.Now.AddDays(-1)),
        });
        Assert.True(bucket.DailyMedian is double);
        Assert.True(bucket.WeeklyMedian is double);
        Assert.True(bucket.MonthlyMedian is double);
        Assert.True(bucket.Deployments is List<DeploymentBucket>);
    }

    [Theory]
    [InlineData(-1, 1, 1, 1)]
    [InlineData(-2, 1, 1, 1)]
    [InlineData(-3, 1, 1, 1)]
    [InlineData(-1, 1, 2, 2)]
    [InlineData(-2, 1, 2, 2)]
    [InlineData(-3, 1, 2, 2)]
    [InlineData(-1, 3, 0, 1)]
    [InlineData(-2, 3, 0, 1)]
    [InlineData(-3, 3, 0, 1)]
    public void TestDeploymentDailyMedian(int interval, int jump, int expected, int nrOfDeployments)
    {
        var deployments = new List<Deployment>();
        var finish = DateTime.Now;
        for (var i = 0; i > 30*interval; i-=jump)
        {
            for (var j = 0; j < nrOfDeployments; j++)
            {
                deployments.Add(Factory.DeploymentFactory(startDate: DateTime.Now.AddDays(i), finishDate: finish));
            }
        }
        var bucket = dfService.CalculateBuckets(interval, deployments);
        Assert.Equal(expected, bucket.DailyMedian);
    }    
    
    [Theory]
    [InlineData(-1, 1, 5)]
    [InlineData(-2, 1, 5)]
    [InlineData(-3, 1, 5)]
    [InlineData(-1, 2, 2)]
    [InlineData(-2, 2, 2)]
    [InlineData(-3, 2, 2)]
    [InlineData(-1, 5, 1)]
    [InlineData(-2, 5, 1)]
    [InlineData(-3, 5, 1)]    
    [InlineData(-1, 12, 0)]
    [InlineData(-2, 12, 0)]
    [InlineData(-3, 12, 0)]
    public void TestDeploymentWeeklyMedian(int interval, int jump, int expected)
    {
        var deployments = new List<Deployment>();
        var finish = DateTime.Now;
        for (var i = 0; i > 30*interval; i-=jump)
        {
            deployments.Add(Factory.DeploymentFactory(startDate: DateTime.Now.AddDays(i), finishDate: finish));
        }
        var bucket = dfService.CalculateBuckets(interval, deployments);
        Assert.Equal(expected, bucket.WeeklyMedian);
    }
    
        
    [Theory]
    [InlineData(-1, 15, 1)]
    [InlineData(-2, 15, 1)]
    [InlineData(-3, 15, 1)]
    public void TestDeploymentMonthlyMedian(int interval, int jump, int expected)
    {
        var deployments = new List<Deployment>();
        var finish = DateTime.Now;
        for (var i = 0; i > 30*interval; i-=jump)
        {
            deployments.Add(Factory.DeploymentFactory(startDate: DateTime.Now.AddDays(i), finishDate: finish));
        }
        var bucket = dfService.CalculateBuckets(interval, deployments);
        Assert.Equal(expected, bucket.MonthlyMedian);
    }
    
    [Fact]
    public void TestIfBucketIsCorrect()
    {
        int interval = -1;
        var bucket = dfService.CalculateBuckets(interval, new List<Deployment>
        {
            Factory.DeploymentFactory(startDate:DateTime.Now.AddDays(-4), finishDate:DateTime.Now.AddDays(-1)),
        });
        bucket.Deployments.ForEach(deploymentBucket => deploymentBucket.DeploymentsInBucket.ForEach(deployment =>
        {
            var startDate = deployment.GetStartDateTime();
            Assert.Equal(deploymentBucket.DayNumber, startDate.Date.Day);
            Assert.Equal(deploymentBucket.WeekNumber, deployment.GetWeek());
            Assert.Equal(deploymentBucket.MonthNumber, startDate.Date.Month);
            Assert.Equal(deploymentBucket.YearNumber, startDate.Date.Year);
        }));
    }
    
}