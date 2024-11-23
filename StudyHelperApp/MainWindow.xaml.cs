using System.Windows;
using StudyHelperApp.Views;

namespace StudyHelperApp
{
    public partial class MainWindow : Window
    {
        public static int? LoggedInUserId { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You are already on the Home page.");
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUserId.HasValue)
            {
                NoteWindow noteWindow = new NoteWindow(subjectId: 1, userId: LoggedInUserId.Value);
                noteWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Please log in to access notes.");
            }
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
            LoggedInUserId = null;
            MessageBox.Show("You have been logged out.");
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
