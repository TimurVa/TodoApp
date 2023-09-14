namespace ToDoApp.Models.CalendarModels
{
    internal sealed class MonthModel : BaseModel
    {
        public int Month { get; set; }

        public int Year { get; set; }   

        public bool IsCurrent { get; set; } = false;

        public string MonthName { get; set; }

        public MonthModel()
        {
            
        }
    }
}
