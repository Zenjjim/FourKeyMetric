
using devops_metrics.Entities;

namespace devops_metrics.Models;

public class RestoreServiceTimeModel
{
    public RestoreServiceTimeModel(double medianRestoreServiceTime, IEnumerable<Weekly> weeklyRestoreServiceTime, IEnumerable<Monthly> monthlyRestoreServiceTime, List<Incident> incidentList)
    {
        this.MedianRestoreServiceTime = !double.IsNaN(medianRestoreServiceTime) ? medianRestoreServiceTime : 0;
        this.WeeklyRestoreServiceTime = weeklyRestoreServiceTime;
        this.MonthlyRestoreServiceTime = monthlyRestoreServiceTime;
        this.Incidents = incidentList;
    }

    public double MedianRestoreServiceTime { get; set; }
    public IEnumerable<Weekly> WeeklyRestoreServiceTime { get; set; }
    public IEnumerable<Monthly> MonthlyRestoreServiceTime { get; set; }
    public List<Incident> Incidents { get; set; }
}


public partial class Monthly
{
    public MonthKey Key { get; set; }
    public double? Median { get; set; }                    
}

public partial class MonthKey
{
    public long? MonthNumber { get; set; }
    public long? YearNumber { get; set; } 
}

public partial class Weekly
{
    public WeekKey Key { get; set; }
    public double? Median { get; set; }                   
}

public partial class WeekKey
{
    public long? WeekNumber { get; set; } 
    public long? YearNumber { get; set; } 
    public long? MonthNumber { get; set; }
}

public partial class DayKey
{
    public long? DayNumber { get; set; } 
    public long? WeekNumber { get; set; } 
    public long? YearNumber { get; set; } 
    public long? MonthNumber { get; set; }
}
