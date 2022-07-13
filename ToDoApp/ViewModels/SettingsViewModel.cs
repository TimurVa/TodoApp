using Microsoft.Data.Sqlite;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToDoApp.Helpers;
using ToDoApp.Interfaces;

namespace ToDoApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel, ISettings
    {
        private bool _isTopMost = true;
        /// <summary>
        /// Is window topmost
        /// </summary>
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
                OnPropertyChanged(nameof(IsTopMost));

                _ = UpdateAsync(1, IsTopMost ? "1" : "0");
            }
        }

        private bool _isLightTheme = true;
        /// <summary>
        /// Is app theme light
        /// </summary>
        public bool IsLightTheme
        {
            get => _isLightTheme;
            set 
            { 
                if (_isLightTheme == value)
                {
                    return;
                }

                _isLightTheme = value;
                OnPropertyChanged();

                UpdateTheme();
            }
        }


        private bool _isDarkTheme;
        /// <summary>
        /// Is app theme dark
        /// </summary>
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set 
            { 
                if (_isDarkTheme == value)
                {
                    return;
                }

                _isDarkTheme = value;
                OnPropertyChanged();

                UpdateTheme();
            }
        }


        #region Constructor
        public SettingsViewModel()
        {
            Task.Run(async () =>
            {
                byte i = 0;
                while (!await CreateTableIfNotExist())
                {
                    System.Diagnostics.Debug.WriteLine("Sqlite busy");
                    await Task.Delay(500);

                    if (i >= Static.REPEAT_COUNT)
                    {
                        throw new Exception("Can't initialize application.");
                    }
                    else
                    {
                        i++;
                    }
                }

                await InitializeAsync();
            });
        }
        #endregion


        #region Helpers
        private void UpdateTheme()
        {
            ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[0];

            if (rd == null)
            {
                return;
            }

            if (_isLightTheme)
            {
                if (rd.Source.ToString() == Static.LIGHT_THEME_PATH)
                {
                    return;
                }

                rd = new ResourceDictionary();
                rd.Source = new Uri(Static.LIGHT_THEME_PATH, UriKind.Relative);

                Application.Current.Resources.MergedDictionaries[0] = rd;

                _ = UpdateAsync(2, "0");
            }
            else if (_isDarkTheme)
            {
                if (rd.Source.ToString() == Static.DARK_THEME_PATH)
                {
                    return;
                }

                rd = new ResourceDictionary();
                rd.Source = new Uri(Static.DARK_THEME_PATH, UriKind.Relative);

                Application.Current.Resources.MergedDictionaries[0] = rd;

                _ = UpdateAsync(2, "1");
            }
        }
        #endregion


        #region Database
        private async Task UpdateAsync(long id, string value, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);
                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand updateTodoItemCommand = sqliteConnection.CreateCommand();
                updateTodoItemCommand.Transaction = transaction;

                updateTodoItemCommand.CommandText = "UPDATE Settings SET Value = @Value WHERE Id = @Id";
                updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Value", value));
                updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", id));

                await updateTodoItemCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (Exception)
            {

            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        private async Task<bool> CreateTableIfNotExist()
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand todoTable = sqliteConnection.CreateCommand();
                todoTable.CommandText = "CREATE TABLE IF NOT EXISTS Settings(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "Property TEXT," +
                    "Value TEXT " +
                    " )";

                await todoTable.ExecuteNonQueryAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        private async Task InitializeAsync()
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                var settingsCommand = sqliteConnection.CreateCommand();

                settingsCommand.CommandText = "SELECT * FROM Settings";
                var reader = await settingsCommand.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        switch (reader.GetString(1))
                        {
                            case nameof(IsTopMost):
                                {
                                    IsTopMost = reader.GetString(2) == "1";
                                    break;
                                }
                            case "Theme":
                                {
                                    string val = reader.GetString(2);

                                    if (val == "0")
                                    {
                                        IsLightTheme = true;
                                        IsDarkTheme = false;
                                    }
                                    else if (val == "1")
                                    {
                                        IsLightTheme = false;
                                        IsDarkTheme = true;
                                    }

                                    break;
                                }
                        }
                    }
                }
                else
                {
                    await reader.CloseAsync().ConfigureAwait(false);

                    IsTopMost = true;
                    IsLightTheme = true;

                    await PopulateSettings(settingsCommand, nameof(IsTopMost), "1");

                    if (IsLightTheme)
                    {
                        await PopulateSettings(settingsCommand, "Theme", "0");
                    }
                    else if (IsDarkTheme)
                    {
                        await PopulateSettings(settingsCommand, "Theme", "1");
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        private async Task PopulateSettings(SqliteCommand settingsCommand, string property, string value)
        {
            settingsCommand.CommandText = $"INSERT INTO Settings (Property, Value) VALUES ('{property}', '{value}')";
            await settingsCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
