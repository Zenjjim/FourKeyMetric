using WorkTimeCalculatorLib;
using WorkTimeCalculatorLib.Models;

static class Utils
{
    public static double CalculateBusinessHours( DateTime startDate, DateTime finishDate )
    {
        var firstShift = new WorkShift() { Start = new TimeSpan(7, 0, 0), End = new TimeSpan(11, 0, 0) };
        var secondShift = new WorkShift() { Start = new TimeSpan(11, 30, 0), End = new TimeSpan(17, 0, 0) };
        
        Dictionary<DayOfWeek, List<WorkShift>> mySchedule = new Dictionary<DayOfWeek, List<WorkShift>>() {
            { DayOfWeek.Monday, new List<WorkShift>(){ firstShift, secondShift } },
            { DayOfWeek.Tuesday, new List<WorkShift>(){ firstShift, secondShift } },
            { DayOfWeek.Wednesday, new List<WorkShift>(){ firstShift, secondShift } },
            { DayOfWeek.Thursday, new List<WorkShift>(){ firstShift, secondShift } },
            { DayOfWeek.Friday, new List<WorkShift>(){ firstShift, secondShift } },
        };
        List<HolidayConfig> myHolidays = new List<HolidayConfig>() { };
        var calculator = new WorkTimeCalculator(mySchedule, myHolidays);
        return calculator.CalculateWorkTime(startDate, finishDate).TotalSeconds;
    }
    
}