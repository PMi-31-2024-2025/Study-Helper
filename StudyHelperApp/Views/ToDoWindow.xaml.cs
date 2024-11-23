using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using Npgsql;
using StudyHelperApp.Data;

namespace StudyHelperApp.Views
{
    public partial class ToDoWindow : Window
    {
        // List to hold tasks
        private List<ToDoTask> tasks = new List<ToDoTask>();

        public ToDoWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        // Load existing tasks (if any)
        private void LoadTasks()
        {
            tasks.Clear(); // Clear the local list of tasks before reloading from the database
            var con1 = DatabaseHelper.ConnectionString;
            var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
            using (var connection = new NpgsqlConnection(con2))
            {
                connection.Open();
                var query = @"
            SELECT task_id, title, is_completed
            FROM ToDoTasks
            WHERE user_id = @user_id
            ORDER BY created_at DESC"; // Assuming tasks are ordered by creation date

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", MainWindow.LoggedInUserId.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new ToDoTask
                            {
                                TaskId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                IsCompleted = reader.GetBoolean(2)
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }

            // Reload tasks in the ListBox
            TasksListBox.Items.Clear();
            foreach (var task in tasks)
            {
                TasksListBox.Items.Add(task);
            }
        }


        // Handle "Add Task" button click
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string taskDescription = TaskTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(taskDescription))
            {
                // Create a new task object
                var newTask = new ToDoTask
                {
                    Description = taskDescription,
                    IsCompleted = false,
                    UserId = MainWindow.LoggedInUserId.Value // Assuming the logged-in user ID is accessible
                };
                var con1 = DatabaseHelper.ConnectionString;
                var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
                // Add the task to the database
                using (var connection = new NpgsqlConnection(con2))
                {
                    connection.Open();
                    var query = @"
                INSERT INTO ToDoTasks (user_id, title, is_completed)
                VALUES (@user_id, @title, @is_completed) RETURNING task_id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", newTask.UserId);
                        command.Parameters.AddWithValue("@title", newTask.Description);
                        command.Parameters.AddWithValue("@is_completed", newTask.IsCompleted);

                        // Execute the query and get the task ID back
                        var result = command.ExecuteScalar();
                        newTask.TaskId = Convert.ToInt32(result);
                    }
                }

                // Add the task to the local list and update the ListBox
                tasks.Add(newTask);
                TaskTextBox.Clear();
                LoadTasks(); // Reload tasks to refresh the list box
            }
            else
            {
                MessageBox.Show("Please enter a task description.");
            }
        }


        // Handle "Mark Completed" button click
        private void MarkCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedItem != null)
            {
                var selectedTask = TasksListBox.SelectedItem as ToDoTask;
                if (selectedTask != null && !selectedTask.IsCompleted)
                {
                    selectedTask.IsCompleted = true;
                    var con1 = DatabaseHelper.ConnectionString;
                    var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
                    // Update the task status in the database
                    using (var connection = new NpgsqlConnection(con2))
                    {
                        connection.Open();
                        var query = @"
                    UPDATE ToDoTasks
                    SET is_completed = @is_completed
                    WHERE task_id = @task_id";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@is_completed", selectedTask.IsCompleted);
                            command.Parameters.AddWithValue("@task_id", selectedTask.TaskId);

                            command.ExecuteNonQuery(); // Execute the update command
                        }
                    }

                    // Reload tasks to update the ListBox
                    LoadTasks();
                }
                else
                {
                    MessageBox.Show("Task is already marked as completed or not selected.");
                }
            }
            else
            {
                MessageBox.Show("Please select a task to mark as completed.");
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            NoteWindow noteWindow = new NoteWindow(subjectId: 1, userId: MainWindow.LoggedInUserId.Value);
            noteWindow.Show();
            Close();
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            GradeWindow gradeWindow = new GradeWindow();
            gradeWindow.Show();
            Close();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }

    // ToDoTask class represents a task with description and completion status
    public class ToDoTask
    {
        public int TaskId { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Description} {(IsCompleted ? "(Completed)" : "(Pending)")}";
        }
    }
}
