using System;

namespace ToDoApp.Models.CalendarModels
{
    internal sealed class DayModel : BaseModel
    {
        /// <summary>
        /// Date of this model
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Starting from 0 to 4
        /// </summary>
        public int WeekNumber { get; set; }

        /// <summary>
        /// Day of week (0 - 7)
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// Determines if this date is from prev month or future.
        /// For ex. currently selected month is September, then
        /// if this is true means that <see cref="Date"/> is August or October
        /// </summary>
        public bool IsNotCurrentMonth { get; set; }

        public DayModel()
        {

        }
    }
}
