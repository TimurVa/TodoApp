using System;
using System.Collections.ObjectModel;
using ToDoApp.Helpers.CustomEventArgs;
using ToDoApp.Models.CalendarModels;

namespace ToDoApp.ViewModels.Calendar
{
    internal sealed class MonthViewModel : BaseViewModel
    {
        public event EventHandler<MontAndYearEventArgs> SelectedMonthChanged;

        private ObservableCollection<MonthModel> _months = new();
        public ObservableCollection<MonthModel> Months
        {
            get => _months;
        }

        private MonthModel _selectedMonth;
        public MonthModel SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth == value) return;

                _selectedMonth = value;
                OnPropertyChanged();
                OnSelectedMonthChanged();
            }
        }

        public MonthViewModel()
        {
            
        }

        public void ShowYear(int year, int month)
        {
            _months.Clear();

            //var date = new DateTime(year, month, 1);

            for (int i = 0; i < 12; i++)
            {
                var model = new MonthModel()
                {
                    Month = i + 1,
                    IsCurrent = i + 1 == month,
                    MonthName = new DateTime(year, i + 1, 1).ToString("MMM"),
                    Year = year,
                };

                _months.Add(model);
            }
        }

        private void OnSelectedMonthChanged()
        {
            if (SelectedMonth == null) return;

            SelectedMonthChanged?.Invoke(this, new MontAndYearEventArgs(_selectedMonth.Year, _selectedMonth.Month));
        }
    }
}
