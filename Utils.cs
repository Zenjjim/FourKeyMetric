static class Utils
{
    public static double GetSecondsInRange(DateTime from, DateTime to)
    {
        TimeSpan rangeStart = new TimeSpan(7, 0, 0);
        TimeSpan rangeEnd = new TimeSpan(17, 0, 0);
        double minutes = 0.0;
        bool overnight = rangeStart > rangeEnd;
        for (var m = from; m < to; m = m.AddMinutes(1)) {
            if (overnight) {
                if (rangeStart <= m.TimeOfDay || m.TimeOfDay < rangeEnd) {
                    minutes++;
                }
            } else {
                if (rangeStart <= m.TimeOfDay) {
                    if (m.TimeOfDay < rangeEnd) {
                        minutes++;
                    } else {
                        break;
                    }
                }
            }
        }
        return minutes*60.0;
    }
}