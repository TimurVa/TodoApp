using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoApp.Controls;
using ToDoApp.Helpers;

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

        private int _ticks = 4;
        public int Ticks
        {
            get => _ticks;
            set
            {
                if (_ticks == value) return;

                _ticks = value;
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

        private ObservableCollection<Sector> _sectors = new ObservableCollection<Sector>();
        public ObservableCollection<Sector> Sectors
        {
            get => _sectors;
            private set => _sectors = value;
        }

        private ObservableCollection<Arrow> _arrows = new ObservableCollection<Arrow>();
        public ObservableCollection<Arrow> Arrows
        {
            get => _arrows;
            private set => _arrows = value;
        }

        private ICommand _createSectorCommand;
        public ICommand CreateSectorCommand => _createSectorCommand ??= new RelayCommand(CreateSectorCommand_Executed);

        private ICommand _clearSectorCommand;
        public ICommand ClearSectorCommand => _clearSectorCommand ??= new RelayCommand(ClearSectorCommand_Executed);

        private ICommand _createArrowCommand;
        public ICommand CreateArrowCommand => _createArrowCommand ??= new RelayCommand(CreateArrowCommand_Executed);

        private double _leftAngle = 15;
        public double LeftAngle
        {
            get => _leftAngle;
            set
            {
                if (_leftAngle == value) return;

                _leftAngle = value;
                OnPropertyChanged();
            }
        }

        private double _rightAngle = 15;
        public double RightAngle
        {
            get => _rightAngle;
            set
            {
                if (_rightAngle == value) return;

                _rightAngle = value;
                OnPropertyChanged();
            }
        }

        private double _lastAngle = 0;
        public double LastAngle
        {
            get => _lastAngle;
            set
            {
                if (_lastAngle == value) return;

                _lastAngle = value;
                OnPropertyChanged();
            }
        }

        private void CreateSectorCommand_Executed(object obj)
        {
            if (Arrows.Count == 0) return;

            if (_rightAngle == 0 && _leftAngle == 0) return;

            if (Sectors.Count > 0)
            {
                var sector = Sectors[0];
                sector.StartAngle = Arrows[^1].Angle - _rnd.Next(1, 180);
                sector.EndAngle = Arrows[^1].Angle + _rnd.Next(1, 180);
                Sectors.Clear();
                Sectors.Add(sector);
            }
            else
            {
                Sectors.Add(new Sector()
                {
                    StartAngle = Arrows[^1].Angle - _rnd.Next(1, 180),
                    EndAngle = Arrows[^1].Angle + _rnd.Next(1, 180),
                });
            }
        }

        Random _rnd = new Random();
        private void CreateArrowCommand_Executed(object obj)
        {
            if (Arrows.Count == 3)
            {
                Arrows.Clear();
            }

            for (int i = 0; i < Arrows.Count; i++)
            {
                Arrows[i].Opacity = (i + 1) / (Arrows.Count + 1.0);
            }

            Arrows.Add(new Arrow()
            {
                Angle = _rnd.Next(0, 360),
                Stroke = "#3c3b6e"
            });

            LastAngle = Arrows[^1].Angle;

            CreateSectorCommand_Executed(null);
        }

        private void ClearSectorCommand_Executed(object obj)
        {
            Sectors = new();
            OnPropertyChanged(nameof(Sectors));

            Arrows = new();
            OnPropertyChanged(nameof(Arrows));
        }
    }
}
