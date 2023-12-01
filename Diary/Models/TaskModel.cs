using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    class TaskModel : INotifyPropertyChanged
    {
        private string _name;
        private string _text = "Описание";
        private bool _isDone;
        private DateTime _executeTime;
        private bool _isImportant;
        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                if (_isDone == value)
                    return;
                _isDone = value;
                OnPropertyChanged("IsDone");
            }
        }
        public bool IsImportent
        {
            get { return _isImportant; }
            set
            {
                if (_isImportant == value)
                    return;
                _isImportant = value;
                OnPropertyChanged("IsImportent");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }
        public DateTime ExecuteTime
        {
            get { return _executeTime; }
            set
            {
                if (_executeTime == value)
                    return;
                _executeTime = value;
                OnPropertyChanged("ExecuteTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
