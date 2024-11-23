using System;
using System.Windows;
using System.Windows.Controls;
using StudyHelperApp.Models;
using System.Collections.ObjectModel;
using Npgsql;
using StudyHelperApp.Data;

namespace StudyHelperApp.Views
{
    public partial class NoteWindow : Window
    {
        private int _subjectId;
        private int _userId;
        private ObservableCollection<Note> Notes { get; set; }

        public NoteWindow(int subjectId, int userId)
        {
            InitializeComponent();
            LoadNotes();
            _subjectId = subjectId;
            _userId = userId;
        }

        private void LoadNotes()
        {
            Notes = new ObservableCollection<Note>();
            var con1 = DatabaseHelper.ConnectionString;
            var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
            using (var connection = new NpgsqlConnection(con2))
            {
                connection.Open();
                var query = "SELECT * FROM Notes WHERE user_id = @user_id ORDER BY created_at DESC";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("user_id", MainWindow.LoggedInUserId.Value);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Notes.Add(new Note
                            {
                                NoteId = reader.GetInt32(0),
                                SubjectId = reader.GetInt32(1),
                                UserId = reader.GetInt32(2),
                                Text = reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4),
                                UpdatedAt = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }
            NotesListView.ItemsSource = Notes;
        }

        private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = NotesListView.SelectedItem as Note;

            // Check if a note is selected (for editing)
            if (selectedNote != null)
            {
                // Update existing note
                selectedNote.Text = NoteTextBox.Text;
                var con1 = DatabaseHelper.ConnectionString;
                var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
                using (var connection = new NpgsqlConnection(con2))
                {
                    connection.Open();
                    var query = "UPDATE Notes SET text = @text, updated_at = CURRENT_TIMESTAMP WHERE note_id = @note_id";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("text", selectedNote.Text);
                        command.Parameters.AddWithValue("note_id", selectedNote.NoteId);
                        command.ExecuteNonQuery();
                    }
                }
                LoadNotes(); // Reload notes after update
                NoteTextBox.Clear(); // Clear the text box after saving
            }
            else
            {
                // No note selected, create a new one
                string noteText = NoteTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(noteText))
                {
                    int userId = MainWindow.LoggedInUserId.Value; // Assuming the user is logged in
                    var con1 = DatabaseHelper.ConnectionString;
                    var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
                    using (var connection = new NpgsqlConnection(con2))
                    {
                        connection.Open();
                        var query = "INSERT INTO Notes (subject_id, user_id, text) VALUES (@subject_id, @user_id, @text)";
                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            // Assume you have a subject_id to associate with the note (for example, 1 here)
                            int subjectId = 1; // Replace with the actual subject ID based on your logic

                            command.Parameters.AddWithValue("@subject_id", subjectId);
                            command.Parameters.AddWithValue("@user_id", userId);
                            command.Parameters.AddWithValue("@text", noteText);

                            // Execute the insertion
                            command.ExecuteNonQuery();
                        }
                    }

                    LoadNotes(); // Reload notes after insertion
                    NoteTextBox.Clear(); // Optionally clear the text box after saving
                }
                else
                {
                    MessageBox.Show("Please enter text for the new note.");
                }
            }
        }

        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = NotesListView.SelectedItem as Note;
            if (selectedNote != null)
            {
                var con1 = DatabaseHelper.ConnectionString;
                var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");
                using (var connection = new NpgsqlConnection(con2))
                {
                    connection.Open();
                    var query = "DELETE FROM Notes WHERE note_id = @note_id";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("note_id", selectedNote.NoteId);
                        command.ExecuteNonQuery();
                    }
                }
                LoadNotes();
                NoteTextBox.Clear(); // Clear the text box after deleting the note
            }
            else
            {
                MessageBox.Show("Please select a note to delete.");
            }
        }


        private void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedNote = NotesListView.SelectedItem as Note;
            if (selectedNote != null)
            {
                NoteTextBox.Text = selectedNote.Text;
            }
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ToDoButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoWindow toDoWindow = new ToDoWindow();
            toDoWindow.Show();
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
            MainWindow.LoggedInUserId = null;
            MessageBox.Show("You have been logged out.");
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
