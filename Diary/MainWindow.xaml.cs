using Diary.Models;
using Diary.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;

namespace Diary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\diaryDataList.json";
        private BindingList<DayModel> _diaryDataList;
        private BindingList<TaskModel> _selectedDayList;
        private SaveWindow _saveWindow;
        private OverlapMessageBox _overlap;
        private List<DayModel> _overlapList;
        private FileIOService _fileIOService;
        private DateTime _consideredDay;
        private bool _sortDirection;
        private string _currentSort;
        
        private string[] _months;

        private const double DATE_BUTTON_WIDTH_PARAM = 0.23;
        private const double PANEL_TOTAL_HEIGHT_MARGE = 21;
        private const double GRID_TOTAL_HEIGHT_MARGE = 100 + PANEL_TOTAL_HEIGHT_MARGE;

        public DateTime ConsideredDay
        {
            get { return _consideredDay; }
            set
            {
                _consideredDay = value;
                _sortDirection = true;          // Sort properties
                _currentSort = "ExecuteDate";   //
                try
                {
                    _selectedDayList = _diaryDataList.Where(day => day.ExecuteDate == _consideredDay).Select(day => day.DiaryList).First();
                    SortDayList(_currentSort);
                }
                catch
                {
                    AddNewDay(_consideredDay);
                }
                taskProgress.Value = SetProgressValue();
                SetButtonProperties(_consideredDay);
                dgDiary.ItemsSource = _selectedDayList;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Добавляет новый день в конец списка и устанавливает его как отображаемый.
        /// </summary>
        /// <param name="date">Дата, которая будет установлена для нового элемента</param>
        private void AddNewDay(DateTime date)
        {
            _diaryDataList.Add(new DayModel(date));
            _selectedDayList = _diaryDataList.Last().DiaryList;
            _selectedDayList.ListChanged += SelectedDayList_ListChanged;
        }
        /// <summary>
        /// Задает новый список задач для выбранной даты.
        /// </summary>
        /// <param name="newPath">Список задач, который будет установлен</param>
        /// <param name="date">Дата, у которой будет изменен список задач</param>
        private void ChangeDayListPath(BindingList<TaskModel> newPath, DateTime date)
        {
            _diaryDataList.Where(day => day.ExecuteDate == date).First().DiaryList = newPath;
        }
        /// <summary>
        /// Изменяет отображаемый список задач.
        /// </summary>
        /// <param name="newPath">Список задач, который будет отображаться</param>
        private void ChangeSelectedDayListPath(BindingList<TaskModel> newPath)
        {
            ChangeDayListPath(newPath, ConsideredDay);
            _selectedDayList = newPath;
            _selectedDayList.ListChanged -= SelectedDayList_ListChanged;
            _selectedDayList.ListChanged += SelectedDayList_ListChanged;
        }
        /// <summary>
        /// Убирает пустые элементы и элементы с пустым значением "Name" из списка.
        /// </summary>
        /// <param name="diaryList">Список с пустыми элементами и элементами с пустым значением "Name"</param>
        /// <returns>Исходный список без пустых элементов и элементов с пустым значением "Name"</returns>
        private BindingList<DayModel> CleanEmptyNotes(BindingList<DayModel> diaryList)
        {
            if (diaryList is null) return null;

            var cleanedList = new BindingList<DayModel>();
            DayModel cleanedDiary;

            foreach (DayModel day in diaryList)
            {
                cleanedDiary = new DayModel(day.ExecuteDate);
                foreach (TaskModel task in day.DiaryList)
                {
                    if (task.Name != string.Empty && task.Name != null)
                        cleanedDiary.DiaryList.Add(task);
                }
                if (cleanedDiary.DiaryList.Count != 0)
                    cleanedList.Add(cleanedDiary);
            }
            return cleanedList;
        }
        /// <summary>
        /// Убирает из списка элементы, где установлена дата ранее текущей.
        /// </summary>
        /// <param name="diaryList">Список, где присутствуют элементы с датой ранее текущей</param>
        /// <returns>Список, где дата у всех элементов старше или равна текущей</returns>
        private BindingList<DayModel> CleanPastNotes(BindingList<DayModel> diaryList)
        {
            if (diaryList is null) return null;

            var newList = new BindingList<DayModel>();
            foreach (DayModel day in diaryList)
            {
                if (day.ExecuteDate >= DateTime.Today)
                    newList.Add(day);
            }
            return newList;
        }
        /// <summary>
        /// Выбирает существующие элементы списка, исподьзуя диапазон дат.
        /// </summary>
        /// <param name="datesRange">Диапазон дат</param>
        /// <returns>Список элементов, которые входят в представленный диапазон</returns>
        private BindingList<DayModel> SelectByDate(List<DateTime> datesRange)
        {
            var newList = new BindingList<DayModel>();
            foreach (DateTime date in datesRange)
            {
                foreach (DayModel day in _diaryDataList)
                {
                    if (day.ExecuteDate.DayOfYear == date.DayOfYear)
                    {
                        newList.Add(day);
                        break;
                    }
                }
            }
            return CleanEmptyNotes(newList);
        }        
        /// <summary>
        /// Создание строки на основе списка DayModel для вывода в текстовый файл.
        /// </summary>
        /// <param name="diaryList"></param>
        /// <returns></returns>
        private string CreateDiaryString(BindingList<DayModel> diaryList)
        {
            string output = string.Empty;
            foreach (DayModel day in diaryList)
            {
                day.DiaryList = new BindingList<TaskModel>(day.DiaryList.OrderBy(task => task.ExecuteTime).ToList());
                output += $"==============  {day.ExecuteDate.Day} {_months[day.ExecuteDate.Month-1]} ==============\n\n";
                foreach (TaskModel task in day.DiaryList)
                {
                    output += task.ExecuteTime.ToString("t");
                    output += task.IsDone ? " (done)\n" : "\n";
                    output += task.Name + "\n";
                    output += task.Text == "Описание" || task.Text == "" ? "\n" : task.Text + "\n\n";
                }
            }

            return output;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(PATH);
            _months = new string[] 
            {
                "JANUARY",
                "FABRUARY",
                "MARCH",
                "APRIL",
                "MAY",
                "JUNE",
                "JULY",
                "AUGUST",
                "SEPTEMBER",
                "OCTOBER",
                "NOVEMBER",
                "DECEMBER"
            };

            try
            {
                _diaryDataList = CleanPastNotes(CleanEmptyNotes(_fileIOService.LoadData())) ?? new BindingList<DayModel>();                
                ConsideredDay = DateTime.Today;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                Close();
            }
            dateCalendar.BlackoutDates.AddDatesInPast();

            if (_selectedDayList.Count > 2)
                Height = _selectedDayList.Count < 6 ? MinHeight + _selectedDayList.Count * 50 : 550;
            else panel.VerticalAlignment = VerticalAlignment.Center;
            FormSizeChanged(ActualWidth * 0.9);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _fileIOService.SaveData(CleanEmptyNotes(_diaryDataList));
        }
        private void SelectedDayList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(_diaryDataList);
                    taskProgress.Value = SetProgressValue();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }              
        private void CloseTooltip_Click(object sender, RoutedEventArgs e)
        {
            dgDiary.UnselectAll();
        }
        private void DateButton_Click(object sender, RoutedEventArgs e)
        {
            if (dateCalendar.Visibility == Visibility.Collapsed)
            {
                mainGrid.Width = new GridLength(275);
                FormSizeChanged(panel.ActualWidth == 710 ? 710 : panel.ActualWidth - 275);
                dateCalendar.Visibility = Visibility.Visible;
            }
            else
            {
                dateCalendar.Visibility = Visibility.Collapsed;
                mainGrid.Width = new GridLength(0);
                FormSizeChanged(panel.ActualWidth + 275 > 710 ? 710 : panel.ActualWidth + 275);
            }
        }        
        private void DateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {            
            ConsideredDay = (DateTime)dateCalendar.SelectedDate;
        }
        private void DateCalendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem) Mouse.Capture(null);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_saveWindow?.IsLoaded == true) return;

            _saveWindow = new SaveWindow(ConsideredDay);
            _saveWindow.saveFileButton.Click += SaveFileButton_Click;
            _saveWindow.Owner = this;
            _saveWindow.Show();
        }
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            BindingList<DayModel> savedList;
            string savedString;
            if (_saveWindow.datesRange != null)
            {
                savedList = SelectByDate(_saveWindow.datesRange);
                if (savedList.Count == 0)
                {
                    System.Windows.MessageBox.Show(this, "Файл не был сохранен, так как список задач для выбранных дней отсутствует.", "Исключение");
                    return;
                }
                try
                {
                    if (_saveWindow.jsonRButton.IsChecked == true)
                        _fileIOService.SaveData(savedList, _saveWindow.Path);
                    else
                    {
                        savedString = CreateDiaryString(savedList);
                        _fileIOService.SaveDoc(savedString, _saveWindow.Path);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    _saveWindow.Close();
                    return;
                }
                System.Windows.MessageBox.Show(this, "Файл успешно сохранен.", "Успешно");
                _saveWindow.Close();
            }
        }        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTasks = dgDiary.SelectedItems;
            int task = 0;
            for (int count = selectedTasks.Count; count > 0; count--)
            {
                if (selectedTasks[task] is TaskModel)
                    _selectedDayList.Remove((TaskModel)selectedTasks[task]);
                else
                    task++;
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            var result = System.Windows.MessageBox.Show(this, "Вы уверены, что хотите очистить лист?", "Внимание", buttons);
            if (result == MessageBoxResult.Yes)
            {
                dgDiary.SelectAll();
                DeleteButton_Click(sender, e);
            }
        }        

        #region ///  Design  ///
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded) return;

            if (panel.VerticalAlignment == VerticalAlignment.Stretch)
            {
                if (dgDiary.ActualHeight + dateButton.ActualHeight + PANEL_TOTAL_HEIGHT_MARGE < panel.ActualHeight)
                {
                    panel.VerticalAlignment = VerticalAlignment.Center;
                    return;
                }
            }
            else if (dgDiary.ActualHeight + dateButton.ActualHeight + GRID_TOTAL_HEIGHT_MARGE >= grid.ActualHeight)
            {
                panel.VerticalAlignment = VerticalAlignment.Stretch;
            }

            FormSizeChanged(panel.ActualWidth);
        }
        private void FormSizeChanged(double panelWidth)
        {
            dgDiary.MaxHeight = grid.ActualHeight - dateButton.Height - GRID_TOTAL_HEIGHT_MARGE;

            dateButton.Width = panelWidth * DATE_BUTTON_WIDTH_PARAM;
            dateButton.Height = dateButton.Width;
            buttonDay.FontSize = dateButton.Height / 2;
            buttonMonth.FontSize = dateButton.Height / 5 + (4 - buttonMonth.Text.Length);
        }
        private void SetButtonProperties(DateTime date)
        {
            buttonDay.Text = date.Day.ToString();
            buttonMonth.Text = _months[date.Month - 1];
            buttonMonth.FontSize = dateButton.Height / 5 + (4 - buttonMonth.Text.Length);
        }
        private double SetProgressValue()
        {
            taskProgress.Maximum = _selectedDayList.Count > 0 ? _selectedDayList.Count : 1;
            double progressCount = 0;
            foreach (TaskModel task in _selectedDayList)
                if (task.IsDone)
                    progressCount++;
            return progressCount;
        }
        #endregion

        #region ///  Sorting  ///
        private void NameSortButton_Click(object sender, RoutedEventArgs e)
        {
            SortDayList("Name");
            dgDiary.ItemsSource = _selectedDayList;
        }
        private void DoneSortButton_Click(object sender, RoutedEventArgs e)
        {
            SortDayList("IsDone");
            dgDiary.ItemsSource = _selectedDayList;
        }
        private void TimeSortButton_Click(object sender, RoutedEventArgs e)
        {
            SortDayList("ExecuteDate");
            dgDiary.ItemsSource = _selectedDayList;
        }
        private void ImportentSortButton_Click(object sender, RoutedEventArgs e)
        {
            SortDayList("IsImportant");
            dgDiary.ItemsSource = _selectedDayList;
        }
        private void SortDayList(string path)
        {
            BindingList<TaskModel> sortedSelectedDayList = null;

            if (path != _currentSort)
            {
                _sortDirection = true;
                _currentSort = path;
            }

            switch (path)
            {
                case "IsDone":
                    if (_sortDirection)
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderBy(task => task.IsDone).ToList());
                    else
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderByDescending(task => task.IsDone).ToList());
                    break;
                case "Name":
                    if (_sortDirection)
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderBy(task => task.Name).ToList());
                    else
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderByDescending(task => task.Name).ToList());
                    break;
                case "ExecuteDate":
                    if (_sortDirection)
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderBy(task => task.ExecuteTime).ToList());
                    else
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderByDescending(task => task.ExecuteTime).ToList());
                    break;
                case "IsImportant":
                    if (_sortDirection)
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderBy(task => task.IsImportent).ToList());
                    else
                        sortedSelectedDayList = new BindingList<TaskModel>(_selectedDayList.OrderByDescending(task => task.IsImportent).ToList());
                    break;
            }

            _sortDirection = !_sortDirection;
            ChangeSelectedDayListPath(sortedSelectedDayList);
        }
        #endregion

        #region ///  Adding  ///
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)| *.json";
            openFileDialog.ShowDialog();
            var fileName = openFileDialog.FileName;
            if (fileName == string.Empty) return;
            try
            {
                var newList = _fileIOService.LoadFromFile(fileName) ?? null;
                UniteLists(newList);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }
        private void UniteLists(object newList)
        {
            _overlapList = new List<DayModel>();
            var overlapModels = new List<OverlapModel>();
            foreach (DayModel newDay in (BindingList<DayModel>)newList)
            {
                bool overlapFound = false;
                foreach (DayModel day in _diaryDataList)
                {
                    if (newDay.ExecuteDate == day.ExecuteDate)
                    {
                        overlapFound = true;
                        _overlapList.Add(newDay);
                        overlapModels.Add(new OverlapModel(day.DiaryList.Count, newDay.DiaryList.Count, newDay.ExecuteDate));
                        break;
                    }
                }
                if (!overlapFound)
                    _diaryDataList.Add(newDay);
            }
            if (_overlapList.Count != 0)
            {
                _overlap = new OverlapMessageBox(overlapModels);
                _overlap.Owner = this;
                _overlap.Closing += Overlap_Closing;
                _overlap.ShowDialog();
            }
        }
        private void Overlap_Closing(object sender, CancelEventArgs e)
        {
            int count = 0;
            int[] overlapOperations = _overlap.Operations;
            foreach (DayModel newDay in _overlapList)
            {
                if (overlapOperations[count] == 0)
                {
                    count++;
                    continue;
                }
                var day = _diaryDataList.Where(diaryDay => diaryDay.ExecuteDate == newDay.ExecuteDate).First();
                if (overlapOperations[count] == 1)
                {
                    foreach (TaskModel task in newDay.DiaryList)
                        day.DiaryList.Add(task);
                }
                else if (overlapOperations[count] == -1)
                {
                    ChangeDayListPath(newDay.DiaryList, newDay.ExecuteDate);
                }
                count++;
            }
            dgDiary.ItemsSource = _diaryDataList;
            ConsideredDay = _consideredDay;
        }
        #endregion

    }
}
