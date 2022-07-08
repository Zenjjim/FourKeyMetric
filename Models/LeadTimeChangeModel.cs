using FourKeyMetrics.Entities;
using FourKeyMetrics.Services.FourKeyService;

namespace FourKeyMetrics.Models;

public class LeadTimeChangeModel
{
    public LeadTimeChangeModel(double medianLeadTimeChange, dynamic weeklyLeadTimeChange, dynamic monthlyLeadTimeChange, List<Change> changesList)
    {
        this.MedianLeadTimeChange = !double.IsNaN(medianLeadTimeChange) ? medianLeadTimeChange : 0;
        this.WeeklyLeadTimeChange = weeklyLeadTimeChange;
        this.MonthlyLeadTimeChange = monthlyLeadTimeChange;
        this.Changes = changesList;
    }

    public double MedianLeadTimeChange { get; set; }
    public dynamic WeeklyLeadTimeChange { get; set; }
    public dynamic MonthlyLeadTimeChange { get; set; }
    public List<Change> Changes { get; set; }
}