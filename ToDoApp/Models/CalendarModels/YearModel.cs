namespace ToDoApp.Models.CalendarModels
{
    internal sealed class YearModel : BaseModel
    {
        public int Year { get; set; }

        private bool _isCurrent = false;
        public bool IsCurrent
        {
            get => _isCurrent;
            set
            {
                if (_isCurrent == value) return;

                _isCurrent = value;
                OnPropertyChanged();
            }
        }
    }
}
