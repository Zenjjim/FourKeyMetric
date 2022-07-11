
using devops_metrics.Entities;

namespace devops_metrics.Models;

public class ChangeFailureRateModel
{
    public ChangeFailureRateModel(IEnumerable<cfrDay> cfrDay, IEnumerable<cfrWeek> cfrWeek, List<cfrMonth> cfrMonth)
    {
        ChangeFailureRateByDay = cfrDay;
        ChangeFailureRateByWeek = cfrWeek;
        ChangeFailureRateByMonth = cfrMonth;
    }

    public ChangeFailureRateModel(IEnumerable<cfrDay> cfrDay, IEnumerable<cfrWeek> cfrWeek, IEnumerable<cfrMonth> cfrMonth)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<cfrDay> ChangeFailureRateByDay{ get; set; }
    public IEnumerable<cfrWeek> ChangeFailureRateByWeek{ get; set; }
    public IEnumerable<cfrMonth> ChangeFailureRateByMonth{ get; set; }
}

public class cfrDay
{
    public long DayNumber { get; set; }  
    public long WeekNumber { get; set; }  
    public long MonthNumber { get; set; }  
    public long YearNumber { get; set; }  
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