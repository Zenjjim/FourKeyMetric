
using devops_metrics.Entities;

namespace devops_metrics.Models;

public class ChangeFailureRateModel
{
    public ChangeFailureRateModel(double changeFailureRate, IEnumerable<cfrDay> changeFailureRateByDay, IEnumerable<cfrWeek> changeFailureRateByWeek, IEnumerable<cfrMonth> changeFailureRateByMonth)
    {
        ChangeFailureRate = changeFailureRate;
        ChangeFailureRateByDay = changeFailureRateByDay;
        ChangeFailureRateByWeek = changeFailureRateByWeek;
        ChangeFailureRateByMonth = changeFailureRateByMonth;
    }

    public double ChangeFailureRate{ get; set; }
    public IEnumerable<cfrDay> ChangeFailureRateByDay{ get; set; }
    public IEnumerable<cfrWeek> ChangeFailureRateByWeek{ get; set; }
    public IEnumerable<cfrMonth> ChangeFailureRateByMonth{ get; set; }
}

public class cfrDay
{
    public DayKey Key { get; set; }
    public double ChangeFailureRate { get; set; }  
}
public class cfrWeek
{
    public WeekKey Key { get; set; }
    public double ChangeFailureRate { get; set; }  
}
public class cfrMonth
{
    public MonthKey Key { get; set; }
    public double ChangeFailureRate { get; set; }  
}