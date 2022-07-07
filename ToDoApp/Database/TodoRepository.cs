using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Database
{
    public class TodoRepository : IRepository<TodoModel>
    {
        private static readonly SemaphoreSlim _s = new SemaphoreSlim(1, 1);
        private const byte REPEAT_COUNT = 1;

        public TodoRepository()
        {
            Task.Run(async () =>
            {
                byte i = 0;
                while(!await CreateTableIfNotExist())
                {
                    System.Diagnostics.Debug.WriteLine("Sqlite busy");
                    await Task.Delay(500);

                    if (i >= REPEAT_COUNT)
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
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand todoTable = sqliteConnection.CreateCommand();
                todoTable.CommandText = "CREATE TABLE IF NOT EXISTS TodoItems(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "Name TEXT, " +
                    "CreatedTimeTics INT64," +
                    "Description TEXT, " +
                    "IsDone BIT," +
                    "DoneTimeTicks INT64" +
                    " )";

                await todoTable.ExecuteNonQueryAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception)
            {
                return false;
            }    
        }

        public async Task<long> AddAsync(TodoModel todo, CancellationToken token = default)
        {
            await _s.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand addCommand = sqliteConnection.CreateCommand();
                addCommand.Transaction = transaction;

                addCommand.CommandText = "INSERT INTO TodoItems (Name, CreatedTimeTics, Description, IsDone)" +
                    "VALUES (" +
                    "@Name," +
                    "@CreatedTimeTics," +
                    "@Description," +
                    "@IsDone" +
                    ")";

                addCommand.Parameters.Add(new SqliteParameter("@Name", todo.Name));
                addCommand.Parameters.Add(new SqliteParameter("@CreatedTimeTics", todo.CreatedTime.ToUniversalTime().Ticks));
                addCommand.Parameters.Add(new SqliteParameter("@Description", todo.Description));
                addCommand.Parameters.Add(new SqliteParameter("@IsDone", todo.IsDone));


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
                _s.Release();
            }

        }

        public async Task DeleteAsync(long id, CancellationToken token = default)
        {
            await _s.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand removeItemCommand = sqliteConnection.CreateCommand();
                removeItemCommand.Transaction = transaction;

                removeItemCommand.CommandText = "DELETE FROM TodoItems WHERE Id = @Id";
                removeItemCommand.Parameters.Add(new SqliteParameter("@Id", id));

                await removeItemCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (Exception e)
            {

            }
            finally
            {
                _s.Release();
            }
        }

        public async Task<TodoModel> GetAsync(long id, CancellationToken token = default)
        {
            await _s.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = "SELECT * FROM TodoItems WHERE Id = @Id";
                getItemsCommand.Parameters.Add(new SqliteParameter("@Id", id));

                using var reader = await getItemsCommand.ExecuteReaderAsync(token).ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (reader.Read())
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
                _s.Release();
            }
        }

        public async Task UpdateAsync(int property, TodoModel todo, CancellationToken token = default)
        {
            await _s.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand updateTodoItemCommand = sqliteConnection.CreateCommand();
                updateTodoItemCommand.Transaction = transaction;

                if (property == 1)
                {
                    updateTodoItemCommand.CommandText = "UPDATE TodoItems SET Name = @Name WHERE Id = @Id";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Name", todo.Name));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
                }
                else if (property == 2)
                {
                    updateTodoItemCommand.CommandText = "UPDATE TodoItems SET CreatedTimeTics = @CreatedTime WHERE Id = @Id";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@CreatedTime", todo.CreatedTime.ToUniversalTime().Ticks));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
                }
                else if (property == 3)
                {
                    updateTodoItemCommand.CommandText = "UPDATE TodoItems SET Description = @Description WHERE Id = @Id";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Description", todo.Description));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
                }
                else if (property == 4)
                {
                    updateTodoItemCommand.CommandText = "UPDATE TodoItems SET IsDone = @IsDone WHERE Id = @Id";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@IsDone", todo.IsDone));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
                }
                else if (property == 5)
                {
                    updateTodoItemCommand.CommandText = "UPDATE TodoItems SET DoneTimeTicks = @DoneTimeTicks WHERE Id = @Id";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@DoneTimeTicks",
                        todo.DoneTime == null ? DBNull.Value : todo.DoneTime?.ToUniversalTime().Ticks));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter("@Id", todo.Id));
                }

                await updateTodoItemCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (Exception e)
            {

            }
            finally
            {
                _s.Release();
            }
        }

        public async IAsyncEnumerable<TodoModel> GetAllAsync()
        {
            await _s.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection("Data Source=appdb.db");
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = "SELECT * FROM TodoItems";

                using var reader = await getItemsCommand.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (reader.Read())
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
                _s.Release();
            }
        }
    }
}
