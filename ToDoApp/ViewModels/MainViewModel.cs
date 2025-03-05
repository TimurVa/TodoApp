using System;
using System.Windows.Input;
using ToDoApp.Helpers;
using ToDoApp.Interfaces;
using ToDoApp.ViewModels.Calendar;

namespace ToDoApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public event EventHandler Completed;
        public event EventHandler Maximize;
        public event EventHandler Minimize;

        #region Properties
        private bool _isDisposed = false;
        private readonly TodoViewModel _todoViewModel;
        private readonly PasswordsViewModel _passwordsViewModel;
        private readonly CalendarViewModel _calendarViewModel;

        private bool _isTopMost = true;
        public bool IsTopMost
        {
            get => _isTopMost;
            set
            {
                if (_isTopMost == value)
                {
                    return;
                }

                _isTopMost = value;
                OnPropertyChanged();
            }
        }

        private readonly ISettings _settings;
        public ISettings SettingsViewModel
        {
            get => _settings;
        }

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            private set
            {
                if (_selectedViewModel == value)
                {
                    return;
                }

                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public MainViewModel(ISettings settings)
        {
            _settings = settings;
            _todoViewModel = new TodoViewModel();
            _passwordsViewModel = new PasswordsViewModel();
            _calendarViewModel = new();

            SelectedViewModel = _todoViewModel;
        }
        #endregion

        #region Commands
        private ICommand _toggleTopMostCommand;
        private ICommand _closeCommand;
        private ICommand _minimizeCommand;
        private ICommand _maximizeCommand;
        private ICommand _showSettingsCommand;
        private ICommand _changePageCommand;

        public ICommand ToggleTopMostCommand => _toggleTopMostCommand ??= new RelayCommand(ToggleTopMostCommand_Executed);
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(CloseCommand_Executed);
        public ICommand MinimizeCommand => _minimizeCommand ??= new RelayCommand(MinimizeCommand_Executed);
        public ICommand MaximizeCommand => _maximizeCommand ??= new RelayCommand(MaximizeCommand_Executed);
        public ICommand ShowSettingsCommand => _showSettingsCommand ??= new RelayCommand(ShowSettingsCommand_Executed);
        public ICommand ChangePageCommand => _changePageCommand ??= new RelayCommand(ChangePageCommand_Executed);

        private void MaximizeCommand_Executed(object obj)
        {
            Maximize?.Invoke(this, EventArgs.Empty);
        }

        private void MinimizeCommand_Executed(object obj)
        {
            Minimize?.Invoke(this, EventArgs.Empty);
        }

        private void CloseCommand_Executed(object obj)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            Completed?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleTopMostCommand_Executed(object obj)
        {
            IsTopMost ^= true;
        }

        private void ShowSettingsCommand_Executed(object obj)
        {
            Ioc.IocContainer.SettingsProvider.Show();
        }

        private void ChangePageCommand_Executed(object obj)
        {
            switch (obj.ToString())
            {
                case "Todo":
                    {
                        SelectedViewModel = _todoViewModel;
                        break;
                    }
                case "Passwords":
                    {
                        SelectedViewModel = _passwordsViewModel;
                        break;
                    }
                case "Calendar":
                    {
                        SelectedViewModel = _calendarViewModel;
                        break;
                    }
            }
        }
        #endregion
    }
}
