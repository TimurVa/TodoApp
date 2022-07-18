using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoApp.Database;
using ToDoApp.Helpers;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class PasswordsViewModel : BaseViewModel
    {
        #region Properties
        private readonly IRepository<PasswordModel> _passwordRepo = Ioc.IocContainer.PasswordRepository;

        private ObservableCollection<PasswordModel> _passwordModels;
        public ObservableCollection<PasswordModel> PasswordModels
        {
            get
            {
                if (_passwordModels == null) _passwordModels = new();

                return _passwordModels;
            }
            private set
            {
                _passwordModels = value;
            }
        }

        private ObservableCollection<PasswordModel> _filteredPasswordModels;
        public ObservableCollection<PasswordModel> FilteredPasswordModels
        {
            get
            {
                if (_filteredPasswordModels == null) _filteredPasswordModels = new();

                return _filteredPasswordModels;
            }
            private set
            {
                _filteredPasswordModels = value;
                OnPropertyChanged();
            }
        }

        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set
            {
                if (_searchString == value)
                {
                    return;
                }

                _searchString = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Constructor
        public PasswordsViewModel()
        {
            PasswordModels.CollectionChanged += PasswordModels_CollectionChanged;
        }

        private void PasswordModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        var item = e.NewItems[0] as PasswordModel;

                        if (item == null)
                        {
                            return;
                        }

                        item.PasswordChanged += Item_PasswordChanged;
                        item.UserNameChanged += Item_UserNameChanged;
                        item.WhatForChanged += Item_WhatForChanged;
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        var item = e.OldItems[0] as PasswordModel;

                        if (item == null)
                        {
                            return;
                        }

                        item.PasswordChanged -= Item_PasswordChanged;
                        item.UserNameChanged -= Item_UserNameChanged;
                        item.WhatForChanged -= Item_WhatForChanged;

                        break;
                    }
            }
        }

        private void Item_WhatForChanged(object sender, EventArgs e)
        {
            _ = UpdateAsync(1, sender as PasswordModel);
        }

        private void Item_UserNameChanged(object sender, EventArgs e)
        {
            _ = UpdateAsync(2, sender as PasswordModel);
        }

        private void Item_PasswordChanged(object sender, EventArgs e)
        {
            _ = UpdateAsync(3, sender as PasswordModel);
        }
        #endregion


        #region Commands
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _copyCommand;
        private ICommand _searchCommand;
        private ICommand _openLinkCommand;
        private ICommand _loadedCommand;

        public ICommand AddCommand => _addCommand ??= new RelayCommand(async (p) => await AddCommand_Executed(p));
        public ICommand RemoveCommand => _removeCommand ??= new RelayCommand(async (p) => await RemoveCommand_Executed(p));
        public ICommand CopyCommand => _copyCommand ??= new RelayCommand(CopyCommand_Executed);
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(SearchCommand_Executed);
        public ICommand OpenLinkCommand => _openLinkCommand ??= new RelayCommand(OpenLinkCommand_Executed);

        /// <summary>
        /// Fired when assosiated Page Loaded event fired.
        /// </summary>
        public ICommand LoadedCommand => _loadedCommand ??= new RelayCommand(async (p) => await LoadedCommand_Executed(p));

        #region Add command
        private async Task AddCommand_Executed(object p)
        {
            PasswordModel pw = new PasswordModel()
            {
                Password = string.Empty,
                UserName = string.Empty,
                WhatFor = string.Empty
            };

            await _passwordRepo.AddAsync(pw).ConfigureAwait(false);

            PasswordModels.Add(pw);
            FilteredPasswordModels.Add(pw);
        }
        #endregion

        #region Delete
        private async Task RemoveCommand_Executed(object obj)
        {
            if (obj == null || obj is not PasswordModel pm)
            {
                return;
            }

            await _passwordRepo.DeleteAsync(pm.Id).ConfigureAwait(false);

            FilteredPasswordModels.Remove(pm);
            PasswordModels.Remove(pm);
        }
        #endregion

        #region Copy password
        private void CopyCommand_Executed(object obj)
        {
            if (obj != null && obj is string str)
            {
                Clipboard.SetText(str);
            }
        }
        #endregion

        #region Search not used
        private void SearchCommand_Executed(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Try open with process provided URI
        private void OpenLinkCommand_Executed(object obj)
        {
            if (obj != null && obj is string str)
            {
                try
                {
                    if (!str.StartsWith("http"))
                    {
                        str = "http://" + str;
                    }

                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = str,
                        UseShellExecute = true
                    };

                    Process.Start(psi);

                    //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    //{
                    //    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
                    //}
                    //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    //{
                    //    Process.Start("xdg-open", url);
                    //}
                    //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    //{
                    //    Process.Start("open", url);
                    //}
                    //else
                    //{
                    //}
                }
                catch (Exception e)
                {
                }
            }
        }
        #endregion

        #region Page loaded 
        private async Task LoadedCommand_Executed(object obj)
        {
            await LoadItems();
        }
        #endregion

        #endregion cmds


        #region Db
        private async Task UpdateAsync(int property, PasswordModel model)
        {
            await _passwordRepo.UpdateAsync(property, model).ConfigureAwait(false);
        }
        #endregion

        /// <summary>
        /// Attempts to load items
        /// </summary>
        /// <returns></returns>
        private async ValueTask LoadItems()
        {
            try
            {
                if (PasswordModels.Count != 0)
                {
                    return;
                }

                // think about loading only few amount of items
                await foreach (var item in _passwordRepo.GetAllAsync().ConfigureAwait(false))
                {
                    PasswordModels.Add(item);
                }

                FilteredPasswordModels = new(PasswordModels);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
