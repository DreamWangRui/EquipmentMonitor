using EquipmentMonitor.Models;
using System;
using System.Data.SQLite;

namespace EquipmentMonitor.Services
{
    public class SqliteStorageService : IStorageService
    {
        private string _dbPath = "sensor.db";

        public void Initialize()
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                string createTable = @"
                        CREATE TABLE IF NOT EXISTS SensorData (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Value REAL NOT NULL,
                            Time DATETIME NOT NULL
                        )";
                new SQLiteCommand(createTable, connection).ExecuteNonQuery();
            }
        }

        public void Save(SensorData data)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                string insert = "INSERT INTO SensorData (Value, Time) VALUES (@value, @time)";
                var command = new SQLiteCommand(insert, connection);
                command.Parameters.AddWithValue("@value", data.Value);
                command.Parameters.AddWithValue("@time", data.Timestamp);
                command.ExecuteNonQuery();
            }
        }
    }
}
