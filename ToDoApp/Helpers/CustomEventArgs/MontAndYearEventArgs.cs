using System;

namespace ToDoApp.Helpers.CustomEventArgs
{
    public sealed class MontAndYearEventArgs : EventArgs
    {
        public int Year { get; }
        public int Month { get; }

        public MontAndYearEventArgs(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
