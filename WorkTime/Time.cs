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
            int allSeconds = seconds + minutes * 60 + hours * 60 * 60;
            int addSeconds = _s + _m * 60 + _h * 60 * 60;
            int totalSeconds = allSeconds + addSeconds;

            hours = totalSeconds / (60 * 60);
            totalSeconds %= (60 * 60);
            minutes = totalSeconds / 60;
            totalSeconds %= 60;
            seconds = totalSeconds;
          
        }

        public void ReduceTime(int _h, int _m, int _s)
        {
            int allSeconds = seconds + minutes * 60 + hours * 60 * 60;
            int reduceSeconds = _s + _m * 60 + _h * 60 * 60;
            int totalSeconds = allSeconds - reduceSeconds;

            if (totalSeconds > 0)
            {
                hours = totalSeconds / (60 * 60);
                totalSeconds %= (60 * 60);
                minutes = totalSeconds / 60;
                totalSeconds %= 60;
                seconds = totalSeconds;
            }
            else
            {
                hours = 0;
                minutes = 0;
                seconds = 0;
            }
        }

        public override string ToString()
        {
            return $"{hours}:{minutes}:{seconds}";
        }




    }
}
