using Diary.Models;
using System.Collections.Generic;
using System.Windows;

namespace Diary
{
    /// <summary>
    /// Логика взаимодействия для OverlapMessageBox.xaml
    /// </summary>
    public partial class OverlapMessageBox : Window
    {
        private readonly List<OverlapModel> _overlapModels;
        private int _operationsCount = 0;
        public int[] Operations { get; private set; }

        public OverlapMessageBox(object overlapModels)
        {
            InitializeComponent();
            if (overlapModels is List<OverlapModel> list)
            {
                _overlapModels = list;
                Operations = new int[_overlapModels.Count];
            }
        }

        public new void ShowDialog()
        {
            CreateOverlapBoxText();
            base.ShowDialog();
        }
        private void CreateOverlapBoxText()
        {
            var currentOverlap = _overlapModels[_operationsCount];
            overlapDate.Text = currentOverlap.Date.ToString("d");
            overlapText.Text = $" уже существует.\n" +
                $"Количество записей с этой датой:\n\t" +
                $"На текущий момент - {currentOverlap.DayTaskCount}.\n\t" +
                $"В выбранном вами файле - {currentOverlap.NewDayTaskCount}." +
                $"\n\nКакую операцию вы хотите провести?";
        }
        private void HandleOverlap(int operation)
        {
            if (repeat.IsChecked == true)
            {
                while (_operationsCount < Operations.Length)
                {
                    Operations[_operationsCount] = operation;
                    _operationsCount++;
                }
                Close();
            }
            else 
            {
                Operations[_operationsCount] = operation;
                _operationsCount++;
                if (_operationsCount == Operations.Length)
                    Close();
                else
                    CreateOverlapBoxText();
            }            
        }

        private void CombineButton_Click(object sender, RoutedEventArgs e)
        {
            HandleOverlap(1);
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            HandleOverlap(-1);
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            HandleOverlap(0);
        }
    }
}
