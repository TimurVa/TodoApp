using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Helpers;
using ToDoApp.Models;

namespace ToDoApp.Database
{
    public class PasswordRepository : IRepository<PasswordModel>
    {
        public PasswordRepository()
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
            });
        }

        private async Task<bool> CreateTableIfNotExist()
        {
            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand table = sqliteConnection.CreateCommand();
                table.CommandText = Constants.CREATE_PASSWORDS_TABLE;

                await table.ExecuteNonQueryAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<long> AddAsync(PasswordModel model, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteTransaction transaction = sqliteConnection.BeginTransaction();

                SqliteCommand addCommand = sqliteConnection.CreateCommand();
                addCommand.Transaction = transaction;

                addCommand.CommandText = Constants.INSERT_PASSWORD;

                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.WHAT_FOR}", model.WhatFor));
                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.USER_NAME}", model.UserName));
                addCommand.Parameters.Add(new SqliteParameter($"@{Constants.PASSWORD}", model.Password));

                await addCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                addCommand.CommandText = "select last_insert_rowid()";
                long result = (long)await addCommand.ExecuteScalarAsync(token).ConfigureAwait(false);

                transaction.Commit();

                model.Id = result;

                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
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

                removeItemCommand.CommandText = Constants.GetDeleteString(Constants.PASSWORDS_TABLE_NAME);
                removeItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", id));

                await removeItemCommand.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                transaction.Commit();
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

        public async IAsyncEnumerable<PasswordModel> GetAllAsync()
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync().ConfigureAwait(false);

                SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = Constants.GetSelectAllString(Constants.PASSWORDS_TABLE_NAME);

                using var reader = await getItemsCommand.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        PasswordModel model = new PasswordModel()
                        {
                            Id = reader.GetInt64(0),
                            WhatFor = reader.GetString(1),
                            UserName = reader.GetString(2)
                        };

                        string passwordHash = reader.GetString(3);

                        if (!string.IsNullOrEmpty(passwordHash))
                        {
                            model.Password = Static.Decrypt(passwordHash, Static.KEY);
                        }

                        // unsecure and assign password

                        await Task.Yield();
                        yield return model;
                    }
                }

                yield break;
            }
            finally
            {
                Static.Semaphore.Release();
            }
        }

        public async Task<PasswordModel> GetAsync(long id, CancellationToken token = default)
        {
            await Static.Semaphore.WaitAsync();

            try
            {
                using SqliteConnection sqliteConnection = new SqliteConnection(Static.DatabasePath);
                await sqliteConnection.OpenAsync(token).ConfigureAwait(false);

                SqliteCommand getItemsCommand = sqliteConnection.CreateCommand();
                getItemsCommand.CommandText = Constants.GetSelectByIdString(Constants.CREATE_PASSWORDS_TABLE);
                getItemsCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", id));

                using var reader = await getItemsCommand.ExecuteReaderAsync(token).ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        PasswordModel model = new PasswordModel()
                        {
                            Id = reader.GetInt64(0),
                            WhatFor = reader.GetString(1),
                            UserName = reader.GetString(2)
                        };

                        string passwordHash = reader.GetString(3);
                        model.Password = Static.Decrypt(passwordHash, Static.KEY);

                        return model;
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

        public async Task UpdateAsync(int property, PasswordModel model, CancellationToken token = default)
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
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.PASSWORDS_TABLE_NAME} SET {Constants.WHAT_FOR} = @{Constants.WHAT_FOR} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.WHAT_FOR}", model.WhatFor));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", model.Id));
                }
                else if (property == 2)
                {
                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.PASSWORDS_TABLE_NAME} SET {Constants.USER_NAME} = @{Constants.USER_NAME} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.USER_NAME}", model.UserName));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", model.Id));
                }
                else if (property == 3)
                {
                    string password = model.Password;

                    string encrypted = Static.Encrypt(password, Static.KEY);

                    updateTodoItemCommand.CommandText = $"UPDATE {Constants.PASSWORDS_TABLE_NAME} SET {Constants.PASSWORD} = @{Constants.PASSWORD} WHERE {Constants.ID} = @{Constants.ID}";
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.PASSWORD}", encrypted));
                    updateTodoItemCommand.Parameters.Add(new SqliteParameter($"@{Constants.ID}", model.Id));
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
    }
}
