using System.Collections.ObjectModel;
using System;
using ToDoApp.Helpers;
using System.Windows.Input;
using ToDoApp.Helpers.CustomEventArgs;
using ToDoApp.Models.CalendarModels;
using System.Diagnostics;

namespace ToDoApp.ViewModels.Calendar
{
    internal sealed class DaysViewModel : BaseViewModel
    {
        public event EventHandler<DayModel> SelectedDayChanged;
        public event EventHandler<MontAndYearEventArgs> MontViewTriggered;

        #region Properties
        private ObservableCollection<DayModel> _dayModels = new();
        public ObservableCollection<DayModel> DayModels
        {
            get => _dayModels;
        }

        private DayModel _selectedDay;
        public DayModel SelectedDay
        {
            get => _selectedDay;
            set
            {
                if (_selectedDay == value) return;

                _selectedDay = value;
                OnPropertyChanged();
                OnSelectedDayChanged(_selectedDay);
            }
        }

        /// <summary>
        /// Currently selected month
        /// </summary>
        private int _month;
        private string _monthStr;
        public string Month
        {
            get => _monthStr;
            set
            {
                if (_monthStr == value) return;

                _monthStr = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Currently selected year
        /// </summary>
        private int _year;
        private string _yearStr;
        public string Year
        {
            get => _yearStr;
            set
            {
                if (_yearStr == value) return;

                _yearStr = value;
                OnPropertyChanged();
            }
        }

        #endregion props


        #region Constructor
        public DaysViewModel()
        {

        }
        #endregion ctor


        #region Commands
        private ICommand _changeMonthCommand;
        private ICommand _showMonthViewCommand;
        public ICommand ChangeMonthCommand => _changeMonthCommand ??= new RelayCommand(ChangeMonthCommand_Executed);
        public ICommand ShowMonthViewCommand => _showMonthViewCommand ??= new RelayCommand(ShowMonthViewCommand_Executed);

        private void ChangeMonthCommand_Executed(object obj)
        {
            if (obj == null) return;

            if (obj is not string str) return;

            bool parseResult = int.TryParse(str, out var val);

            if (!parseResult) return;

            ShowMonth(_month + val, _year);
        }

        private void ShowMonthViewCommand_Executed(object p)
        {
            MontViewTriggered?.Invoke(this, new MontAndYearEventArgs(_year, _month));
        }
        #endregion cmds


        private void OnSelectedDayChanged(DayModel selectedDay)
        {
            SelectedDayChanged?.Invoke(this, selectedDay);
        }

        public void ShowMonth(int month, int year)
        {
            Debug.WriteLine($"{month}-{year}");
            if (month == _month &&  year == _year) return;

            if (month > 12)
            {
                month = 1;
                year++;
            }

            if (month < 1)
            {
                month = 12;
                year--;
            }
            
            DayModels.Clear();
            DateTime startDate = new(year, month, 1);
            DateTime startDate2 = new(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            DateTime endDate2 = startDate.AddMonths(1);//.AddDays(-1);
            Month = startDate.ToString("MMMM");
            Year = startDate2.ToString("yyyy");
            _month = startDate.Month;
            _year = startDate2.Year;
            int lastWeekNumber = 0;

            DayModel? lastDay = null;
            while (startDate <= endDate)
            {
                var dayOfWeek = startDate.DayOfWeek;
                lastWeekNumber = startDate.GetWeekOfMonth();

                DayModels.Add(new DayModel
                {
                    Date = startDate,
                    WeekNumber = lastWeekNumber,
                    WeekDay = dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1
                });
                lastDay = DayModels[^1];
                if (startDate == DateTime.MinValue) return;
                startDate = startDate.AddDays(1);

                //Debug.WriteLine(lastWeekNumber + " " + DayModels[DayModels.Count - 1].Date.ToShortDateString());
            }

            if (startDate2.DayOfWeek == DayOfWeek.Monday)
            {
                goto endDate;
            }

            if (startDate2 == DateTime.MinValue) return;
            startDate2 = startDate2.AddDays(-1);
            while (true)
            {
                var dayOfWeek = startDate2.DayOfWeek;

                DayModels.Add(new DayModel
                {
                    Date = startDate2,
                    WeekNumber = 0,
                    WeekDay = dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1,
                    IsNotCurrentMonth = true
                });

                if (startDate2 == DateTime.MinValue) return;
                startDate2 = startDate2.AddDays(-1);

                if (startDate2.DayOfWeek == DayOfWeek.Sunday) break;
            }

            endDate:
            if (lastDay.WeekDay == 6)
            {
                lastWeekNumber++;
            }
            while (true)
            {
                var dayOfWeek = endDate2.DayOfWeek;

                DayModels.Add(new DayModel
                {
                    Date = endDate2,
                    WeekNumber = lastWeekNumber,
                    WeekDay = dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1,
                    IsNotCurrentMonth = true
                });

                if (endDate2 == DateTime.MinValue) return;
                endDate2 = endDate2.AddDays(1);

                if (endDate2.DayOfWeek == DayOfWeek.Monday)
                {
                    if (lastWeekNumber >= 5)
                    {
                        return;
                    }

                    lastWeekNumber++;
                }
            }
        }
    }
}
