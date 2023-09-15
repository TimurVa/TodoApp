using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoApp.Helpers;
using ToDoApp.Helpers.CustomEventArgs;
using ToDoApp.Models.CalendarModels;

namespace ToDoApp.ViewModels.Calendar
{
    internal sealed class MonthViewModel : BaseViewModel
    {
        public event EventHandler<MontAndYearEventArgs> SelectedMonthChanged;
        public event EventHandler<MontAndYearEventArgs> YearViewTriggered;

        #region Properties
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

        private int _currentYear;
        private string _year;
        public string Year
        {
            get => _year;
            private set
            {
                if (_year == value) return;

                _year = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Constructor
        public MonthViewModel()
        {
            
        }
        #endregion


        #region Commands
        private ICommand _changeYearCommand;
        private ICommand _showYearViewCommand;
        public ICommand ChangeYearCommand => _changeYearCommand ??= new RelayCommand(ChangeYearCommand_Executed);
        public ICommand ShowYearViewCommand => _showYearViewCommand ??= new RelayCommand(ShowYearViewCommand_Executed);

        private void ChangeYearCommand_Executed(object p)
        {
            if (p is null) return;

            if (p is not string str) return;

            bool parseResult = int.TryParse(str, out var year);

            if (!parseResult) return;

            ShowYear(_currentYear + year);
        }

        private void ShowYearViewCommand_Executed(object p)
        {
            YearViewTriggered?.Invoke(this, new MontAndYearEventArgs(_currentYear, 1));
        }
        #endregion


        public void ShowYear(int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year) return;

            _months.Clear();

            //var date = new DateTime(year, month, 1);

            for (int i = 0; i < 12; i++)
            {
                var model = new MonthModel()
                {
                    Month = i + 1,
                    IsCurrent = DateTime.UtcNow.Month == i + 1 && year == DateTime.UtcNow.Year,
                    MonthName = new DateTime(year, i + 1, 1).ToString("MMM"),
                    Year = year,
                };

                _months.Add(model);
            }

            Year = new DateTime(year, 1, 1).ToString("yyyy");
            _currentYear = year;
        }

        private void OnSelectedMonthChanged()
        {
            if (SelectedMonth == null) return;

            SelectedMonthChanged?.Invoke(this, new MontAndYearEventArgs(_selectedMonth.Year, _selectedMonth.Month));
        }
    }
}
