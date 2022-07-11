
using devops_metrics.Entities;

namespace devops_metrics.Models;

public class RestoreServiceTimeModel
{
    public RestoreServiceTimeModel(double medianRestoreServiceTime, dynamic weeklyRestoreServiceTime, dynamic monthlyRestoreServiceTime, List<Incident> incidentList)
    {
        this.MedianRestoreServiceTime = !double.IsNaN(medianRestoreServiceTime) ? medianRestoreServiceTime : 0;
        this.WeeklyRestoreServiceTime = weeklyRestoreServiceTime;
        this.MonthlyRestoreServiceTime = monthlyRestoreServiceTime;
        this.Incidents = incidentList;
    }

    public double MedianRestoreServiceTime { get; set; }
    public dynamic WeeklyRestoreServiceTime { get; set; }
    public dynamic MonthlyRestoreServiceTime { get; set; }
    public List<Incident> Incidents { get; set; }
}