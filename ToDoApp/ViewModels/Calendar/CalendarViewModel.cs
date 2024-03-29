﻿using System;

namespace ToDoApp.ViewModels.Calendar
{
    internal class CalendarViewModel : BaseViewModel
    {
        private readonly DaysViewModel _days;
        private readonly MonthViewModel _months;
        private readonly YearViewModel _years;
        //public DaysViewModel Days
        //{
        //    get => _days;
        //    set
        //    {
        //        if (Days == value) return;

        //        _days = value;
        //        OnPropertyChanged();
        //    }
        //}

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            private set
            {
                if (_selectedViewModel == value) return;

                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        public CalendarViewModel()
        {
            _days = new DaysViewModel();
            DateTime current = DateTime.UtcNow;
            _days.ShowMonth(current.Month, current.Year);
            _days.MontViewTriggered += Days_MontViewTriggered;

            _selectedViewModel = _days;

            _months = new MonthViewModel();
            _months.SelectedMonthChanged += Months_SelectedMonthChanged;
            _months.YearViewTriggered += Months_YearViewTriggered;

            _years = new YearViewModel();
            _years.SelectedYearChanged += Years_SelectedYearChanged;
        }

        private void Years_SelectedYearChanged(object sender, Helpers.CustomEventArgs.MontAndYearEventArgs e)
        {
            _months.ShowYear(e.Year);
            SelectedViewModel = _months;
        }

        private void Months_YearViewTriggered(object sender, Helpers.CustomEventArgs.MontAndYearEventArgs e)
        {
            _years.ShowYears(e.Year);
            SelectedViewModel = _years;
        }

        private void Months_SelectedMonthChanged(object sender, Helpers.CustomEventArgs.MontAndYearEventArgs e)
        {
            _days.ShowMonth(e.Month, e.Year);
            SelectedViewModel = _days;
        }

        private void Days_MontViewTriggered(object sender, Helpers.CustomEventArgs.MontAndYearEventArgs e)
        {
            _months.ShowYear(e.Year);
            SelectedViewModel = _months;
        }
    }
}
