using System;
using System.Windows.Input;
using ToDoApp.Helpers;
using ToDoApp.Interfaces;

namespace ToDoApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public event EventHandler Completed;
        public event EventHandler Maximize;
        public event EventHandler Minimize;

        #region Properties
        //private bool? _lastFilterByIsDone;
        private bool _isDisposed = false;
        //private readonly IRepository<TodoModel> _todoRepo = Ioc.IocContainer.TodoRepository;
        private readonly TodoViewModel _todoViewModel;
        private readonly PasswordsViewModel _passwordsViewModel;

        //private ObservableCollection<TodoModel> _todoModels;
        //public ObservableCollection<TodoModel> TodoModels
        //{
        //    get
        //    {
        //        if (_todoModels == null) _todoModels = new ObservableCollection<TodoModel>();

        //        return _todoModels;
        //    }
        //    private set
        //    {
        //        _todoModels = value;
        //    }
        //}

        //private ObservableCollection<TodoModel> _filteredTodoModels;
        //public ObservableCollection<TodoModel> FilteredTodoModels
        //{
        //    get
        //    {
        //        if (_filteredTodoModels == null) _filteredTodoModels = new ObservableCollection<TodoModel>();

        //        return _filteredTodoModels;
        //    }
        //    private set
        //    {
        //        _filteredTodoModels = value;
        //        OnPropertyChanged();
        //    }
        //}

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

            SelectedViewModel = _todoViewModel;

            //TodoModels.CollectionChanged += _todoModels_CollectionChanged;

            //Task.Run(async () =>
            //{
            //    await LoadItems();
            //});

        }

        //private void _todoModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    switch (e.Action)
        //    {
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
        //            {
        //                var item = e.NewItems[0] as TodoModel;

        //                if (item == null)
        //                {
        //                    return;
        //                }

        //                item.NameChangedEvent += Item_NameChangedEvent;
        //                item.DescriptionChangedEvent += Item_DescriptionChangedEvent;
        //                item.DoneChangedEvent += Item_DoneChangedEvent;
        //                break;
        //            }
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
        //            {
        //                var item = e.OldItems[0] as TodoModel;

        //                if (item == null)
        //                {
        //                    return;
        //                }

        //                item.NameChangedEvent -= Item_NameChangedEvent;
        //                item.DescriptionChangedEvent -= Item_DescriptionChangedEvent;
        //                item.DoneChangedEvent -= Item_DoneChangedEvent;
        //                break;
        //            }
        //    }
        //}

        //private void Item_DescriptionChangedEvent(object sender, EventArgs e)
        //{
        //    _ = UpdateTodoItem(3, (TodoModel)sender);
        //}

        //private void Item_NameChangedEvent(object sender, EventArgs e)
        //{
        //    _ = UpdateTodoItem(1, (TodoModel)sender);
        //}

        //private void Item_DoneChangedEvent(object sender, EventArgs e)
        //{
        //    _ = UpdateTodoItem(5, (TodoModel)sender);
        //}
        #endregion

        #region Commands
        //private ICommand _addNewTodoItemCommand;
        //private ICommand _removeTodoItemCommand;
        //private ICommand _setTodoItemAsDoneCommand;
        //private ICommand _filterTodoItemsCommand;
        private ICommand _toggleTopMostCommand;
        private ICommand _closeCommand;
        private ICommand _minimizeCommand;
        private ICommand _maximizeCommand;
        private ICommand _showSettingsCommand;
        private ICommand _changePageCommand;

        //public ICommand AddNewTodoItemCommand => _addNewTodoItemCommand ??= new RelayCommand(async (p) => await AddNewTodoItemCommand_Executed(p));
        //public ICommand RemoveTodoItemCommand => _removeTodoItemCommand ??= new RelayCommand(async(p) => await RemoveTodoItemCommand_Executed(p));
        //public ICommand SetTodoItemAsDoneCommand => _setTodoItemAsDoneCommand ??= new RelayCommand(async(p) => await SetTodoItemAsDoneCommand_Executed(p));
        //public ICommand FilterTodoItemsCommand => _filterTodoItemsCommand ??= new RelayCommand(FilterTodoItemsCommand_Executed);
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

        //private async Task AddNewTodoItemCommand_Executed(object obj)
        //{
        //    TodoModel todoModel = new TodoModel()
        //    {
        //        CreatedTime = DateTime.Now,
        //        Description = "Description",
        //        IsDone = false,
        //        Name = "Name"
        //    };

        //    TodoModels.Insert(0, todoModel);

        //    if (FilteredTodoModels.Count == 0 || TodoModels.Count - 1 == FilteredTodoModels.Count)
        //    {
        //        FilteredTodoModels.Insert(0, todoModel);
        //    }
        //    else if (_lastFilterByIsDone == false)
        //    {
        //        FilteredTodoModels.Insert(0, todoModel);
        //    }

        //    await _todoRepo.AddAsync(todoModel);
        //    return;

        //    using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
        //    sqliteConnection.Open();

        //    using SqliteTransaction transaction = sqliteConnection.BeginTransaction();

        //    SqliteCommand addCommand = sqliteConnection.CreateCommand();
        //    addCommand.Transaction = transaction;
        //    addCommand.CommandText = "INSERT INTO TodoItems (Name, CreatedTimeTics, Description, IsDone)" +
        //        "VALUES (" +
        //        "@Name," +
        //        "@CreatedTimeTics," +
        //        "@Description," +
        //        "@IsDone" +
        //        ")";

        //    addCommand.Parameters.Add(new SqliteParameter("@Name", todoModel.Name));
        //    addCommand.Parameters.Add(new SqliteParameter("@CreatedTimeTics", todoModel.CreatedTime.ToUniversalTime().Ticks));
        //    addCommand.Parameters.Add(new SqliteParameter("@Description", todoModel.Description));
        //    addCommand.Parameters.Add(new SqliteParameter("@IsDone", todoModel.IsDone));


        //    addCommand.ExecuteNonQuery();

        //    addCommand.CommandText = "select last_insert_rowid()";
        //    long result = (long)addCommand.ExecuteScalar();

        //    transaction.Commit();

        //    todoModel.Id = result;
        //}

        //private async Task RemoveTodoItemCommand_Executed(object obj)
        //{
        //    if (obj == null || obj is not TodoModel tm)
        //    {
        //        return;
        //    }

        //    TodoModels.Remove(tm);

        //    bool contains = false;
        //    foreach (var item in FilteredTodoModels)
        //    {
        //        if (item == tm)
        //        {
        //            contains = true;
        //            break;
        //        }
        //    }

        //    if (contains)
        //    {
        //        FilteredTodoModels.Remove(tm);
        //    }

        //    await _todoRepo.DeleteAsync(tm.Id);
        //    return;

        //    using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
        //    sqliteConnection.Open();

        //    SqliteCommand removeItemCommand = sqliteConnection.CreateCommand();

        //    removeItemCommand.CommandText = "DELETE FROM TodoItems WHERE Id = @Id";
        //    removeItemCommand.Parameters.Add(new SqliteParameter("@Id", tm.Id));

        //    removeItemCommand.ExecuteNonQuery();
        //}

        //private async Task SetTodoItemAsDoneCommand_Executed(object obj)
        //{
        //    if (obj == null || obj is not TodoModel tm)
        //    {
        //        return;
        //    }

        //    tm.IsDone ^= true;

        //    await UpdateTodoItem(4, tm);
        //}

        //private async Task UpdateTodoItem(int property, TodoModel todo, CancellationToken token = default)
        //{
        //    await _todoRepo.UpdateAsync(property, todo);
        //    return;

        //    using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
        //    await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

        //    SqliteCommand updateTodoItemCommand = sqliteConnection.CreateCommand();

        //    if (property == 1)
        //    {
        //        updateTodoItemCommand.CommandText = "UPDATE TodoItems SET Name = @Name WHERE Id = @Id";
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Name", todo.Name));
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
        //    }
        //    else if (property == 2)
        //    {
        //        updateTodoItemCommand.CommandText = "UPDATE TodoItems SET CreatedTimeTics = @CreatedTime WHERE Id = @Id";
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@CreatedTime", todo.CreatedTime.ToUniversalTime().Ticks));
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
        //    }
        //    else if (property == 3)
        //    {
        //        updateTodoItemCommand.CommandText = "UPDATE TodoItems SET Description = @Description WHERE Id = @Id";
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Description", todo.Description));
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
        //    }
        //    else if (property == 4)
        //    {
        //        updateTodoItemCommand.CommandText = "UPDATE TodoItems SET IsDone = @IsDone WHERE Id = @Id";
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@IsDone", todo.IsDone));
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
        //    }
        //    else if (property == 5)
        //    {
        //        updateTodoItemCommand.CommandText = "UPDATE TodoItems SET DoneTimeTicks = @DoneTimeTicks WHERE Id = @Id";
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@DoneTimeTicks", 
        //            todo.DoneTime == null ? DBNull.Value : todo.DoneTime?.ToUniversalTime().Ticks));
        //        updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
        //    }

        //    await updateTodoItemCommand.ExecuteNonQueryAsync(token);
        //}

        //private void FilterTodoItemsCommand_Executed(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return;
        //    }

        //    switch (obj.ToString())
        //    {
        //        case "0":
        //            {
        //                DropFilters();
        //                break;
        //            }
        //        case "1":
        //            {
        //                FilterByIsDone();
        //                break;
        //            }
        //        case "2":
        //            {
        //                break;
        //            }
        //    }
        //}

        //private void DropFilters()
        //{
        //    if (FilteredTodoModels.Count == TodoModels.Count)
        //    {
        //        _lastFilterByIsDone = null;
        //        return;
        //    }

        //    FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);
        //}

        //private void FilterByIsDone()
        //{
        //    if (_lastFilterByIsDone == true)
        //    {
        //        _lastFilterByIsDone = false;

        //        FilteredTodoModels.Clear();

        //        foreach (var item in TodoModels)
        //        {
        //            if (item.IsDone == false)
        //            {
        //                FilteredTodoModels.Add(item);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        _lastFilterByIsDone = true;

        //        FilteredTodoModels.Clear();

        //        foreach (var item in TodoModels)
        //        {
        //            if (item.IsDone)
        //            {
        //                FilteredTodoModels.Add(item);
        //            }
        //        }
        //    }
        //}

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
            }
        }
        #endregion

        //private async Task LoadItems()
        //{
        //    try
        //    {
        //        await foreach (var item in _todoRepo.GetAllAsync())
        //        {
        //            TodoModels.Insert(0, item);
        //        }

        //        FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);

        //        return;
        //        using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
        //        await sqliteConnection.OpenAsync().ConfigureAwait(false);

        //        SqliteCommand todoTable = sqliteConnection.CreateCommand();
        //        todoTable.CommandText = "CREATE TABLE IF NOT EXISTS TodoItems(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
        //            "Name TEXT, " +
        //            "CreatedTimeTics INT64," +
        //            "Description TEXT, " +
        //            "IsDone BIT," +
        //            "DoneTimeTicks INT64" +
        //            " )";

        //        todoTable.ExecuteNonQuery();

        //        SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
        //        getItemsCommand.CommandText = "SELECT * FROM TodoItems";

        //        using var reader = await getItemsCommand.ExecuteReaderAsync().ConfigureAwait(false);

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                DateTime time = new DateTime(reader.GetInt64(2));

        //                time = time.Add(DateTimeOffset.Now.Offset);

        //                TodoModel todoModel = new TodoModel()
        //                {
        //                    Id = reader.GetInt64(0),
        //                    CreatedTime = time,
        //                    Description = reader.GetString(3),
        //                    IsDone = reader.GetBoolean(4),
        //                    Name = reader.GetString(1)
        //                };

        //                long? tics = reader.IsDBNull(5) ? null : reader.GetInt64(5);

        //                if (tics == null)
        //                {
        //                    todoModel.DoneTime = null;
        //                }

        //                TodoModels.Insert(0, todoModel);
        //            }

        //            FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.WriteLine(e.ToString());
        //    }

        //}
    }
}
