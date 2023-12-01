using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    class OverlapModel
    {
        public int DayTaskCount { get; set; }
        public int NewDayTaskCount { get; set; }
        public DateTime Date { get; set; }
        public OverlapModel(int dayTaskCount, int newDayTaskCount, DateTime date)
        {
            DayTaskCount = dayTaskCount;
            NewDayTaskCount = newDayTaskCount;
            Date = date;
        }
    }
}
