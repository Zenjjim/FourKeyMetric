using Itenso.TimePeriod;

static class Utils
{
    public static double CalculateBusinessHours( DateTime startDate, DateTime finishDate )
    {
        CalendarTimeRange period = new CalendarTimeRange(startDate, finishDate);
        CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
        filter.AddWorkingWeekDays(); // only working days
        filter.CollectingHours.Add( new HourRange( 7, 17 ) );  // opening hours morning
 
        CalendarPeriodCollector collector = new CalendarPeriodCollector( 
            filter, period );
        collector.CollectHours();
 
        double businessHours = 0.0;
        foreach ( var timePeriod in collector.Periods )
        {
            var IPeriod = (ICalendarTimeRange)timePeriod;
            businessHours += Math.Round( IPeriod.Duration.TotalHours, 2 );
        }
        return businessHours;
    }
    
    
}