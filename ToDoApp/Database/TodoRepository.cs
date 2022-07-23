using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Helpers;
using ToDoApp.Models;

namespace ToDoApp.Database
{
    public class TodoRepository : IRepository<TodoModel>
    {
        public TodoRepository()
        {
            Task.Run(async () =>
            {
                byte i = 0;
                while(!await CreateTableIfNotExist())
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
            });
        }

        private async Task<bool> CreateTableIfNotExist()
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand todoTable = sqliteConnection.CreateCommand();
                todoTable.CommandText = Constants.CREATE_TODO_TABLE;

                await todoTable.ExecuteNonQueryAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }    
        }

        public async Task<long> AddAsync(TodoModel todo, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand addCommand = sqliteConnection.CreateCommand();
                addCommand.Transaction = transaction;

                addCommand.CommandText = $"INSERT INTO {Constants.TODO_TABLE_NAME} ({Constants.NAME}, {Constants.CREATED_TIME_TICKS}, {Constants.DESCRIPTION}, {Constants.IS_DONE})" +
                    "VALUES (" +
                    $"@{Constants.NAME}," +
                    $"@{Constants.CREATED_TIME_TICKS}," +
                    $"@{Constants.DESCRIPTION}," +
                    $"@{Constants.IS_DONE}" +
                    ")";

                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.NAME}", todo.Name));
                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.CREATED_TIME_TICKS}", todo.CreatedTime.ToUniversalTime().Ticks));
                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.DESCRIPTION}", todo.Description));
                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.IS_DONE}", todo.IsDone));


                await addCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                addCommand.CommandText = "select last_insert_rowid()";
                long result = (long)await addCommand.ExecuteScalarAsync(token).ConfigureAwait(false);

                transaction.Commit();

                todo.Id = result;

                return result;
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        public async Task DeleteAsync(long id, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand removeItemCommand = sqliteConnection.CreateCommand();
                removeItemCommand.Transaction = transaction;

                removeItemCommand.CommandText = Constants.GetDeleteString(Constants.TODO_TABLE_NAME);
                removeItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", id));

                await removeItemCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (Exception e)
            {

            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        public async Task<TodoModel> GetAsync(long id, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = Constants.GetSelectByIdString(Constants.TODO_TABLE_NAME);
                getItemsCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", id));

                using var reader = await getItemsCommand.ExecuteReaderAsync(token).ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false ))
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

                        return todoModel;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        public async Task UpdateAsync(int property, TodoModel todo, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand updateTodoItemCommand = sqliteConnection.CreateCommand();
                updateTodoItemCommand.Transaction = transaction;

                if (property == 1)
                {
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.TODO_TABLE_NAME} SET {Constants.NAME} = @{Constants.NAME} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.NAME}", todo.Name));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", todo.Id));
                }
                else if (property == 2)
                {
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.TODO_TABLE_NAME} SET {Constants.CREATED_TIME_TICKS} = @{Constants.CREATED_TIME_TICKS} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.CREATED_TIME_TICKS}", todo.CreatedTime.ToUniversalTime().Ticks));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", todo.Id));
                }
                else if (property == 3)
                {
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.TODO_TABLE_NAME} SET {Constants.DESCRIPTION} = @{Constants.DESCRIPTION} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.DESCRIPTION}", todo.Description));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", todo.Id));
                }
                else if (property == 4)
                {
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.TODO_TABLE_NAME} SET {Constants.IS_DONE} = @{Constants.IS_DONE} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.IS_DONE}", todo.IsDone));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", todo.Id));
                }
                else if (property == 5)
                {
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.TODO_TABLE_NAME} SET {Constants.DONE_TIME_TICKS} = @{Constants.DONE_TIME_TICKS} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.DONE_TIME_TICKS}",
                        todo.DoneTime == null ? DBNull.Value : todo.DoneTime?.ToUniversalTime().Ticks));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", todo.Id));
                }

                await updateTodoItemCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (Exception e)
            {

            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        public async IAsyncEnumerable<TodoModel> GetAllAsync()
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = Constants.GetSelectAllString(Constants.TODO_TABLE_NAME);

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
                }

                yield break;
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }
    }
}
