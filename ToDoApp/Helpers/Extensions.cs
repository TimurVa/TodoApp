using System;
using System.Globalization;

namespace ToDoApp.Helpers
{
    internal static class Extensions
    {
        static readonly GregorianCalendar _gc = new();
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new(time.Year, time.Month, 1);

            return time.GetWeekOfYear() - first.GetWeekOfYear();
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
