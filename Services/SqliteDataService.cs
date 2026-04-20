using EquipmentMonitor.Models;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace EquipmentMonitor.Services
{
    public class SqliteDataService : IDataService
    {
        private readonly string _connectionString = "Data Source=sensor.db";

        public SqliteDataService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
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

        public Task SaveAsync(SensorData data)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string insert = "INSERT INTO SensorData (Value, Time) VALUES (@value, @time)";
                var command = new SQLiteCommand(insert, connection);
                command.Parameters.AddWithValue("@value", data.Value);
                command.Parameters.AddWithValue("@time", data.Timestamp);
                command.ExecuteNonQuery();
            }

            return Task.CompletedTask;
        }
    }
}
