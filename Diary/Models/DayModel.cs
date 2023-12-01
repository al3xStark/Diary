using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    class DayModel
    {
        private BindingList<TaskModel> _tasks;
        private DateTime _executeDate;

        public DayModel(DateTime day)
        {
            _executeDate = day;
            _tasks = new BindingList<TaskModel>();
        }
        public DateTime ExecuteDate
        {
            get { return _executeDate; }
            set
            {
                if (_executeDate == value)
                    return;
                _executeDate = value;
            }
        }
        public BindingList<TaskModel> DiaryList
        {
            get { return _tasks; }
            set
            {
                if (_tasks == value)
                    return;
                _tasks = value;
            }
        }        
    }
}
