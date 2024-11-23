using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Npgsql;
using StudyHelperApp.Data;

namespace StudyHelperApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            if (AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!");

                // Set the logged-in user ID
                MainWindow.LoggedInUserId = GetUserId(username);

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                if (RegisterUser(username, password))
                {
                    MessageBox.Show("User not found. New user created and logged in successfully!");

                    // Set the logged-in user ID after registration
                    MainWindow.LoggedInUserId = GetUserId(username);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("An error occurred during user registration.");
                }
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            var con1 = DatabaseHelper.ConnectionString;
            var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
            using (var connection = new NpgsqlConnection(con2))
            {
                connection.Open();
                string query = "SELECT password_hash FROM Users WHERE username = @username";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var storedHash = command.ExecuteScalar() as string;

                    if (storedHash == null)
                    {
                        return false;
                    }

                    return VerifyPassword(password, storedHash);
                }
            }
        }

        private bool RegisterUser(string username, string password)
        {
            var con1 = DatabaseHelper.ConnectionString;
            var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
            using (var connection = new NpgsqlConnection(con2))
            {
                connection.Open();

                string hashedPassword = HashPassword(password);

                string insertQuery = @"
                    INSERT INTO Users (username, password_hash, email, role, created_at)
                    VALUES (@username, @password_hash, @username || '@example.com', 'User', CURRENT_TIMESTAMP)";

                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", hashedPassword);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashString = Convert.ToBase64String(hashBytes);

                return hashString == storedHash;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private int GetUserId(string username)
        {
            var con1 = DatabaseHelper.ConnectionString;
            var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
            using (var connection = new NpgsqlConnection(con2))
            {
                connection.Open();
                string query = "SELECT user_id FROM Users WHERE username = @username";  // Changed 'id' to 'user_id'

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1; // Return the user ID or -1 if not found
                }
            }
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        // Navigate to the notes page (if logged in)
        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.LoggedInUserId.HasValue)
            {
                NoteWindow noteWindow = new NoteWindow(subjectId: 1, userId: MainWindow.LoggedInUserId.Value);
                noteWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Please log in to access notes.");
            }
        }

        // Navigate to the to-do list page
        private void ToDoButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoWindow toDoWindow = new ToDoWindow();
            toDoWindow.Show();
            Close();
        }

        // This button is already on the grades page, so just show a message
        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            GradeWindow gradeWindow = new GradeWindow();
            gradeWindow.Show();
            Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoggedInUserId = null;
            MessageBox.Show("You have been logged out.");
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
