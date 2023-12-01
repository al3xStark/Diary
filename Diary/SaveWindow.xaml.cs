using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Diary
{
    /// <summary>
    /// Логика взаимодействия для SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window
    {
        public List<DateTime> datesRange;
        public string Path { get; set; }

        public SaveWindow(DateTime selectedDate)
        {
            InitializeComponent();
            saveDataCalendar.SelectedDate = selectedDate;
            saveDataCalendar.DisplayDate = selectedDate;
            pathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDataCalendar.BlackoutDates.AddDatesInPast();
        }

        private void SaveDataCalendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem) Mouse.Capture(null);
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            pathTextBox.Text = folderBrowserDialog.SelectedPath;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileNameTextBox.Text != string.Empty && pathTextBox.Text != string.Empty)
            {
                if (jsonRButton.IsChecked == true)
                    Path = pathTextBox.Text + "\\" + fileNameTextBox.Text + ".json";
                else
                    Path = pathTextBox.Text + "\\" + fileNameTextBox.Text + ".doc";
                datesRange = saveDataCalendar.SelectedDates.ToList();
            }
            else
            {
                if (fileNameTextBox.Text == string.Empty)
                    fileNameTextBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD1, 0xD1));
                if (pathTextBox.Text == string.Empty)
                    pathTextBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD1, 0xD1));
            }
        }

        private void PathTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            pathTextBox.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
        }
        private void FileNameTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            fileNameTextBox.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
        }
    }
}
