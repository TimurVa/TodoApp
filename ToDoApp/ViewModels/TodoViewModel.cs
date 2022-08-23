using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Database;
using ToDoApp.Helpers;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        #region Properties
        private readonly IRepository<TodoModel> _todoRepo = Ioc.IocContainer.TodoRepository;
        private bool? _lastFilterByIsDone;
        private string _lastSearchMessage;

        private ObservableCollection<TodoModel> _todoModels;
        public ObservableCollection<TodoModel> TodoModels
        {
            get
            {
                if (_todoModels == null) _todoModels = new ObservableCollection<TodoModel>();

                return _todoModels;
            }
            private set
            {
                _todoModels = value;
            }
        }

        private ObservableCollection<TodoModel> _filteredTodoModels;
        public ObservableCollection<TodoModel> FilteredTodoModels
        {
            get
            {
                if (_filteredTodoModels == null) _filteredTodoModels = new ObservableCollection<TodoModel>();

                return _filteredTodoModels;
            }
            private set
            {
                _filteredTodoModels = value;
                OnPropertyChanged();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
           {
                if (_searchText == value)
                    return;

                _searchText = value;
                OnPropertyChanged();

                if (string.IsNullOrEmpty(SearchText))
                    SearchCommand.Execute(null);
            }
        }
        #endregion


        #region Constructor
        public TodoViewModel()
        {
            TodoModels.CollectionChanged += _todoModels_CollectionChanged;

            Task.Run(async () =>
            {
                await LoadItems();
            });
        }

        private void _todoModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        var item = e.NewItems[0] as TodoModel;

                        if (item == null)
                        {
                            return;
                        }

                        item.NameChangedEvent += Item_NameChangedEvent;
                        item.DescriptionChangedEvent += Item_DescriptionChangedEvent;
                        item.DoneChangedEvent += Item_DoneChangedEvent;
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        var item = e.OldItems[0] as TodoModel;

                        if (item == null)
                        {
                            return;
                        }

                        item.NameChangedEvent -= Item_NameChangedEvent;
                        item.DescriptionChangedEvent -= Item_DescriptionChangedEvent;
                        item.DoneChangedEvent -= Item_DoneChangedEvent;
                        break;
                    }
            }
        }

        private void Item_DescriptionChangedEvent(object sender, EventArgs e)
        {
            _ = UpdateTodoItem(3, (TodoModel)sender);
        }

        private void Item_NameChangedEvent(object sender, EventArgs e)
        {
            _ = UpdateTodoItem(1, (TodoModel)sender);
        }

        private void Item_DoneChangedEvent(object sender, EventArgs e)
        {
            _ = UpdateTodoItem(5, (TodoModel)sender);
        }
        #endregion


        #region Commands
        private ICommand _addNewTodoItemCommand;
        private ICommand _removeTodoItemCommand;
        private ICommand _setTodoItemAsDoneCommand;
        private ICommand _filterTodoItemsCommand;
        private ICommand _searchCommand;
        private ICommand _loadMoreItems;

        public ICommand AddNewTodoItemCommand => _addNewTodoItemCommand ??= new RelayCommand(async (p) => await AddNewTodoItemCommand_Executed(p));
        public ICommand RemoveTodoItemCommand => _removeTodoItemCommand ??= new RelayCommand(async (p) => await RemoveTodoItemCommand_Executed(p));
        public ICommand SetTodoItemAsDoneCommand => _setTodoItemAsDoneCommand ??= new RelayCommand(async (p) => await SetTodoItemAsDoneCommand_Executed(p));
        public ICommand FilterTodoItemsCommand => _filterTodoItemsCommand ??= new RelayCommand(FilterTodoItemsCommand_Executed);
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(SearchCommand_Executed, SearchCommand_CanExecute);
        public ICommand LoadMoreItemsCommand => _loadMoreItems ??= new RelayCommand(async (p) => await LoadMoreItemsCommand_Executed(p));

        private async Task AddNewTodoItemCommand_Executed(object obj)
        {
            TodoModel todoModel = new TodoModel()
            {
                CreatedTime = DateTime.Now,
                Description = "Description",
                IsDone = false,
                Name = "Name"
            };

            long result = await _todoRepo.AddAsync(todoModel);

            if (result == -1)
            {
                return;
            }

            TodoModels.Insert(0, todoModel);

            if (FilteredTodoModels.Count == 0 || TodoModels.Count - 1 == FilteredTodoModels.Count)
            {
                FilteredTodoModels.Insert(0, todoModel);
            }
            else if (_lastFilterByIsDone == false)
            {
                FilteredTodoModels.Insert(0, todoModel);
            }

        }

        private async Task RemoveTodoItemCommand_Executed(object obj)
        {
            if (obj == null || obj is not TodoModel tm)
            {
                return;
            }

            TodoModels.Remove(tm);

            bool contains = false;
            foreach (var item in FilteredTodoModels)
            {
                if (item == tm)
                {
                    contains = true;
                    break;
                }
            }

            if (contains)
            {
                FilteredTodoModels.Remove(tm);
            }

            await _todoRepo.DeleteAsync(tm.Id);
        }

        private async Task SetTodoItemAsDoneCommand_Executed(object obj)
        {
            if (obj == null || obj is not TodoModel tm)
            {
                return;
            }

            tm.IsDone ^= true;

            await UpdateTodoItem(4, tm);

            if (_lastFilterByIsDone == true)
            {
                if (tm.IsDone == true)
                {
                    FilteredTodoModels.Remove(tm);
                }
            }
            else if (_lastFilterByIsDone == false)
            {
                if (tm.IsDone == false)
                {
                    FilteredTodoModels.Remove(tm);
                }
            }
        }

        private async Task<bool> Get()
        {
            await Task.Delay(5000);
            return true;
        }

        private async Task UpdateTodoItem(int property, TodoModel todo, CancellationToken token = default)
        {
            await _todoRepo.UpdateAsync(property, todo);
        }

        private void FilterTodoItemsCommand_Executed(object obj)
        {
            if (obj == null)
            {
                return;
            }

            switch (obj.ToString())
            {
                case "0":
                    {
                        DropFilters();
                        break;
                    }
                case "1":
                    {
                        if (_lastFilterByIsDone == null)
                        {
                            _lastFilterByIsDone = true;
                        }
                        else
                        {
                            _lastFilterByIsDone ^= true;
                        }

                        Filter();
                        break;
                    }
                case "2":
                    {
                        break;
                    }
            }
        }

        private void SearchCommand_Executed(object p)
        {
            if (string.IsNullOrEmpty(_lastSearchMessage) && 
                string.IsNullOrEmpty(_searchText) || 
                string.Equals(_lastSearchMessage, _searchText))
            {
                return;
            }

            if (string.IsNullOrEmpty(_searchText))
            {
                _lastSearchMessage = _searchText;

                if (_lastFilterByIsDone == null)
                {
                    FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);
                }
                else
                {
                    Filter();
                }

                return;
            }

            Filter();

            _lastSearchMessage = SearchText;
        }

        private bool SearchCommand_CanExecute(object p)
        {
            return !string.IsNullOrEmpty(SearchText);
        }

        private void DropFilters()
        {
            _lastFilterByIsDone = null;
            _searchText = string.Empty;
            OnPropertyChanged(nameof(SearchText));

            if (FilteredTodoModels.Count == TodoModels.Count)
            {
                return;
            }

            FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);
        }

        private void Filter()
        {
            if (_lastFilterByIsDone == true)
            {
                //_lastFilterByIsDone = false;

                FilteredTodoModels.Clear();

                foreach (var item in TodoModels)
                {
                    if (item.IsDone == false)
                    {
                        if (!string.IsNullOrEmpty(SearchText))
                        {
                            if (item.Description.Contains(SearchText))
                            {
                                FilteredTodoModels.Add(item);
                            }
                        }
                        else
                        {
                            FilteredTodoModels.Add(item);
                        }
                    }
                }
            }
            else if (_lastFilterByIsDone == false)
            {
                //_lastFilterByIsDone = true;

                FilteredTodoModels.Clear();

                foreach (var item in TodoModels)
                {
                    if (item.IsDone)
                    {
                        if (!string.IsNullOrEmpty(SearchText))
                        {
                            if (item.Description.Contains(SearchText))
                            {
                                FilteredTodoModels.Add(item);
                            }
                        }
                        else
                        {
                            FilteredTodoModels.Add(item);
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(SearchText))
            {
                FilteredTodoModels.Clear();

                foreach (var item in TodoModels)
                {
                    if (item.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    {
                        FilteredTodoModels.Add(item);
                    }
                }
            }
            else if (FilteredTodoModels.Count == TodoModels.Count)
            {
                return;
            }
            else
            {
                FilteredTodoModels.Clear();

                FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);
            }
        }


        #region Load more items
        private async Task LoadMoreItemsCommand_Executed(object p)
        {
            await LoadItems();
        }
        #endregion

        #endregion cmds

        private async Task LoadItems()
        {
            try
            {
                if (_searchText != string.Empty || _lastFilterByIsDone != null)
                {
                    return;
                }

                int count = TodoModels.Count;
                await foreach (var item in LoadMore().ConfigureAwait(false))
                {
                    TodoModels.Add(item);
                }

                if (TodoModels.Count == count)
                {
                    return;
                }

                FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);

                //await foreach (var item in _todoRepo.GetAllAsync())
                //{
                //    TodoModels.Insert(0, item);
                //}

                //FilteredTodoModels = new ObservableCollection<TodoModel>(TodoModels);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private async IAsyncEnumerable<TodoModel> LoadMore()
        {
            //System.Diagnostics.Debug.WriteLine("LoadMore " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            int loaded_count = _todoModels.Count;

            await Static.Semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                using SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = $"select * from {Constants.TODO_TABLE_NAME} order by ROWID desc LIMIT 10 OFFSET {loaded_count}";

                using var reader = await getItemsCommand.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        DateTime time = new DateTime(reader.GetInt64(2));

                        time = time.Add(DateTimeOffset.Now.Offset);

                        TodoModel todoModel = new TodoModel()
                        {
                            Id = reader.GetInt64(0),
                            CreatedTime = time,
                            Description = reader.GetString(3),
                            IsDone = reader.GetBoolean(4),
                            Name = reader.GetString(1)
                        };

                        long? tics = reader.IsDBNull(5) ? null : reader.GetInt64(5);

                        if (tics == null)
                        {
                            todoModel.DoneTime = null;
                        }
                        await Task.Yield();
                        yield return todoModel;
                    }

                    yield break;
                }
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }
    }
}
