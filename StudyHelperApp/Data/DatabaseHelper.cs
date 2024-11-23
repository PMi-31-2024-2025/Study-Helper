using Npgsql;
using System;
using System.IO;

namespace StudyHelperApp.Data
{
    public static class DatabaseHelper
    {
        static DatabaseHelper()
        {
            // Specify the path to the .env file here
            string envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
            Load(envPath);
        }

        // Connection string loaded from environment variable
        public static string ConnectionString { get; } =
            Environment.GetEnvironmentVariable("DATABASE_URL");

        // Method to load environment variables from a .env file
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue; // Skip empty lines and comments

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                    continue; // Skip lines that are not key-value pairs

                var key = parts[0].Trim();
                var value = parts[1].Trim();
                Environment.SetEnvironmentVariable(key, value);
                var env = Environment.GetEnvironmentVariables();
                var con = Environment.GetEnvironmentVariable("DATABASE_URL");
            }
        }
        public static void InitializeDatabase()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            // Create tables if not exist
            var commands = new[]
            {
                @"
                CREATE TABLE IF NOT EXISTS Users (
                    user_id SERIAL PRIMARY KEY,
                    username TEXT NOT NULL UNIQUE,
                    password_hash TEXT NOT NULL,
                    email TEXT NOT NULL UNIQUE,
                    role TEXT DEFAULT 'User',
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );",
                @"
                CREATE TABLE IF NOT EXISTS Subjects (
                    subject_id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL,
                    name TEXT NOT NULL,
                    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
                );",
                @"
                CREATE TABLE IF NOT EXISTS Notes (
                    note_id SERIAL PRIMARY KEY,
                    subject_id INT NOT NULL,
                    user_id INT NOT NULL,
                    text TEXT,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id) ON DELETE CASCADE,
                    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
                );",
                @"
                CREATE TABLE IF NOT EXISTS ToDoTasks (
                    task_id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL,
                    title TEXT NOT NULL,
                    is_completed BOOLEAN DEFAULT FALSE,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE
                );",
                @"
                CREATE TABLE IF NOT EXISTS Grades (
                    grade_id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL,
                    subject_id INT NOT NULL,
                    grade INT NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
                    FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id) ON DELETE CASCADE
                );"
            };

            foreach (var commandText in commands)
            {
                using var command = new NpgsqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
