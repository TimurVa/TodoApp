namespace ToDoApp.Helpers
{
    public class Constants
    {
        public static string CREATE_PASSWORDS_TABLE = $"CREATE TABLE IF NOT EXISTS {PASSWORDS_TABLE_NAME}({ID} INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    $"{WHAT_FOR} TEXT, " +
                    $"{USER_NAME} TEXT," +
                    $"{PASSWORD} TEXT" +
                    " )";

        public static string INSERT_PASSWORD = $"INSERT INTO Passwords ({WHAT_FOR}, {USER_NAME}, {PASSWORD})" +
                    "VALUES (" +
                    $"@{WHAT_FOR}," +
                    $"@{USER_NAME}," +
                    $"@{PASSWORD}" +
                    ")";

        public static string CREATE_TODO_TABLE = $"CREATE TABLE IF NOT EXISTS {TODO_TABLE_NAME}({ID} INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    $"{NAME} TEXT, " +
                    $"{CREATED_TIME_TICKS} INT64," +
                    $"{DESCRIPTION} TEXT, " +
                    $"{IS_DONE} BIT," +
                    $"{DONE_TIME_TICKS} INT64" +
                    " )";

        public static string CREATE_SETTINGS_TABLE = $"CREATE TABLE IF NOT EXISTS {Constants.SETTINGS_TABLE_NAME}({Constants.ID} INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    $"{PROPERTY} TEXT," +
                    $"{VALUE} TEXT " +
                    " )";

        public const string PASSWORDS_TABLE_NAME = "Passwords";
        public const string TODO_TABLE_NAME = "TodoItems";
        public const string SETTINGS_TABLE_NAME = "Settings";
        public const string VALUE = "Value";
        public const string PROPERTY = "Property";
        public const string ID = "Id";
        public const string WHAT_FOR = "WhatFor";
        public const string USER_NAME = "UserName";
        public const string PASSWORD = "Password";
        public const string NAME = "Name";
        public const string CREATED_TIME_TICKS = "CreatedTimeTics";
        public const string DESCRIPTION = "Description";
        public const string IS_DONE = "IsDone";
        public const string DONE_TIME_TICKS = "DoneTimeTicks";

        public static string GetDeleteString(string tableName)
        {
            return $"DELETE FROM {tableName} WHERE {ID} = @{ID}";
        }

        public static string GetSelectAllString(string tableName)
        {
            return $"SELECT * FROM {tableName}";
        }

        public static string GetSelectByIdString(string tableName)
        {
            return $"SELECT * FROM {tableName} WHERE {ID} = @{ID}";
        }
    }
}
