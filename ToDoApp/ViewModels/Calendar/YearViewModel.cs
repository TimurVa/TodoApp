using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoApp.Helpers;
using ToDoApp.Helpers.CustomEventArgs;
using ToDoApp.Models.CalendarModels;

namespace ToDoApp.ViewModels.Calendar
{
    internal sealed class YearViewModel : BaseViewModel
    {
        public event EventHandler<MontAndYearEventArgs> SelectedYearChanged;

        #region Properties
        private ObservableCollection<YearModel> _yearModels = new();
        public ObservableCollection<YearModel> YearModels
        {
            get => _yearModels;
            private set
            {
                _yearModels = value;
                OnPropertyChanged();
            }
        }

        private YearModel _selectedYear;
        public YearModel SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear == value) return;

                _selectedYear = value;
                OnPropertyChanged();
                OnSelectedYearChanged();
            }
        }

        private int _currentYear;
        private string _yearRange;
        public string YearRange
        {
            get => _yearRange;
            private set
            {
                if (_yearRange == value) return;

                _yearRange = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Constructor
        public YearViewModel()
        {
            
        }
        #endregion


        #region Commands
        private ICommand _changeYearCommand;
        public ICommand ChangeYearCommand => _changeYearCommand ??= new RelayCommand(ChangeeYearCommand_Executed);

        private void ChangeeYearCommand_Executed(object p)
        {
            if (p is null) return;

            if (p is not string str) return;

            bool parseResult = int.TryParse(str, out var year);

            ShowYears(_currentYear + year);
        }
        #endregion


        public void ShowYears(int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year) return;

            _yearModels.Clear();

            for (int i = 0; i < 12; i++)
            {
                var model = new YearModel();

                if (i < 6)
                {
                    int y = year - (6 - i);

                    if (y < DateTime.MinValue.Year)
                    {
                        model.Year = year + i;
                        _yearModels.Insert(i, model);
                    }
                    else
                    {
                        model.Year = y;
                        _yearModels.Add(model);
                    }
                }
                else
                {
                    int y = year + (i - 6);   

                    if (y > DateTime.MaxValue.Year)
                    {
                        model.Year = year - (i - 6);
                        _yearModels.Insert(0, model);
                    }
                    else
                    {
                        model.Year = y;
                        _yearModels.Add(model);
                    }
                }

                model.IsCurrent = DateTime.UtcNow.Year == model.Year;
            }

            YearRange = _yearModels[0].Year.ToString() + '-' + _yearModels[11].Year.ToString();
            _currentYear = year;
        }

        private void OnSelectedYearChanged()
        {
            if (_selectedYear == null) return;

            SelectedYearChanged?.Invoke(this, new MontAndYearEventArgs(_selectedYear.Year, 1));
        }
    }
}
