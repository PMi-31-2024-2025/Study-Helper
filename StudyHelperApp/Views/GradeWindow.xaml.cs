using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using StudyHelperApp.Models;
using Npgsql;
using StudyHelperApp.Data;
using System.Windows.Controls;

namespace StudyHelperApp.Views
{
    public partial class GradeWindow : Window
    {
        private ObservableCollection<Grade> Grades { get; set; }
        private ObservableCollection<SubjectAverage> SubjectAverages { get; set; }

        public GradeWindow()
        {
            InitializeComponent();
            LoadGrades();
            CalculateSubjectAverages();
            GenerateBarChart();
        }

        // Load grades for the logged-in user
        private void LoadGrades()
        {
            Grades = new ObservableCollection<Grade>();
            var con2 = Environment.GetEnvironmentVariable("DATABASE_URL");

            using (var connection = new NpgsqlConnection(con2))
            {
                connection.Open();
                var query = @"
                    SELECT g.grade_id, g.grade, g.created_at, s.name
                    FROM Grades g
                    INNER JOIN Subjects s ON g.subject_id = s.subject_id
                    WHERE g.user_id = @user_id
                    ORDER BY g.created_at DESC";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", MainWindow.LoggedInUserId.Value);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Grades.Add(new Grade
                            {
                                GradeId = reader.GetInt32(0),
                                Value = reader.GetInt32(1),
                                CreatedAt = reader.GetDateTime(2),
                                SubjectName = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            GradesListView.ItemsSource = Grades;  // Bind grades to the ListView
        }

        // Calculate average grades for each subject
        private void CalculateSubjectAverages()
        {
            SubjectAverages = new ObservableCollection<SubjectAverage>();

            var averages = Grades
                .GroupBy(g => g.SubjectName)
                .Select(group => new SubjectAverage
                {
                    SubjectName = group.Key,
                    AverageGrade = group.Average(g => g.Value)
                });

            foreach (var avg in averages)
            {
                SubjectAverages.Add(avg);
            }

            AverageGradesListView.ItemsSource = SubjectAverages;  // Bind averages to the ListView
        }

        // Generate a bar chart for average grades
        private void GenerateBarChart()
        {
            BarChartCanvas.Children.Clear();

            if (SubjectAverages.Any())
            {
                double canvasHeight = BarChartCanvas.Height;
                double maxGrade = SubjectAverages.Max(s => s.AverageGrade);
                double barWidth = 50;
                double spacing = 20;
                double xPosition = spacing;

                foreach (var subject in SubjectAverages)
                {
                    // Calculate bar height based on the max grade
                    double barHeight = (subject.AverageGrade / maxGrade) * canvasHeight;

                    // Create the bar
                    var bar = new Rectangle
                    {
                        Fill = Brushes.MediumPurple,
                        Width = barWidth,
                        Height = barHeight
                    };

                    // Position the bar
                    Canvas.SetLeft(bar, xPosition);
                    Canvas.SetTop(bar, canvasHeight - barHeight);

                    // Add the bar to the canvas
                    BarChartCanvas.Children.Add(bar);

                    // Add label below the bar for the subject name
                    var subjectLabel = new TextBlock
                    {
                        Text = subject.SubjectName,
                        FontSize = 10,
                        TextAlignment = TextAlignment.Center,
                        Width = barWidth,
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.Wrap
                    };

                    Canvas.SetLeft(subjectLabel, xPosition);
                    Canvas.SetTop(subjectLabel, canvasHeight); // Slightly below the canvas
                    BarChartCanvas.Children.Add(subjectLabel);

                    // Add a label at the top of the bar for the average grade
                    var gradeLabel = new TextBlock
                    {
                        Text = subject.AverageGrade.ToString("F1"), // Format grade to 1 decimal place
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Black,
                        TextAlignment = TextAlignment.Center,
                        Width = barWidth
                    };

                    Canvas.SetLeft(gradeLabel, xPosition);
                    Canvas.SetTop(gradeLabel, canvasHeight - barHeight - 20); // Slightly above the bar
                    BarChartCanvas.Children.Add(gradeLabel);

                    xPosition += barWidth + spacing;
                }
            }
        }

        // Navigation methods (same as earlier)

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

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

        private void ToDoButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoWindow toDoWindow = new ToDoWindow();
            toDoWindow.Show();
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

    public class SubjectAverage
    {
        public string SubjectName { get; set; }
        public double AverageGrade { get; set; }
    }
}
