using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime
{
    class Time
    {
        private int hours;
        private int minutes;
        private int seconds;

        public string GetTime()
        {
            return $"{hours}:{minutes}:{seconds}";
        }

        public Time()
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
        }
        public Time(int _h, int _m, int _s)
        {
            hours = _h;
            minutes = _m;
            seconds = _s;
        }

        public void AddTime(int _h, int _m, int _s)
        {
            seconds += _s;
            int addMinutes = seconds / 60;
            seconds = seconds % 60;
            minutes += (_m + addMinutes);
            int addHours = minutes / 60;
            minutes = minutes % 60;
            hours += (_h + addHours);            
        }

        public void SubstractTime(int _h, int _m, int _s)
        {

        }

        public override string ToString()
        {
            return $"{hours}:{minutes}:{seconds}";
        }




    }
}
