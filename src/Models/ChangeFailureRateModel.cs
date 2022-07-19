
using devops_metrics.Entities;

namespace devops_metrics.Models;

public class ChangeFailureRateModel
{
    public ChangeFailureRateModel(double changeFailureRate, IEnumerable<ChangeFailureRateDay> changeFailureRateByDay, IEnumerable<ChangeFailureRateWeek> changeFailureRateByWeek, IEnumerable<ChangeFailureRateMonth> changeFailureRateByMonth)
    {
        ChangeFailureRate = changeFailureRate;
        ChangeFailureRateByDay = changeFailureRateByDay;
        ChangeFailureRateByWeek = changeFailureRateByWeek;
        ChangeFailureRateByMonth = changeFailureRateByMonth;
    }

    public double ChangeFailureRate{ get; set; }
    public IEnumerable<ChangeFailureRateDay> ChangeFailureRateByDay{ get; set; }
    public IEnumerable<ChangeFailureRateWeek> ChangeFailureRateByWeek{ get; set; }
    public IEnumerable<ChangeFailureRateMonth> ChangeFailureRateByMonth{ get; set; }
}

public class ChangeFailureRateDay
{
    public DayKey Key { get; set; }
    public double ChangeFailureRate { get; set; }  
}
public class ChangeFailureRateWeek
{
    public WeekKey Key { get; set; }
    public double ChangeFailureRate { get; set; }  
}
public class ChangeFailureRateMonth
{
    public MonthKey Key { get; set; }
    public double ChangeFailureRate { get; set; }  
}