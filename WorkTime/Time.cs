using System;

namespace WorkTime
{
    class Time
    {
        private int hours = 0;
        private int minutes = 0;
        private int seconds = 0;

        public string GetTime()
        {
            return $"{hours}:{minutes}:{seconds}";
        }

        public Time()
        {
          
        }
        public Time(int _h, int _m, int _s)
        {
            AddTime(_h, _m, _s);
        }

        public static Time Now()
        {
            DateTime now = DateTime.Now;
            return new Time(now.Hour, now.Minute, now.Second);
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

        public static Time operator -(Time t1, Time t2)
        {
            Time result = new Time(t1.hours, t1.minutes, t1.seconds);
            result.ReduceTime(t2.hours, t2.minutes, t2.seconds);
            return result;
        }

        public static bool operator >=(Time t1, Time t2)
        {
            int t1AllSeconds = t1.seconds + t1.minutes * 60 + t1.hours * 60 * 60;
            int t2AllSeconds = t2.seconds + t2.minutes * 60 + t2.hours * 60 * 60;
            return t1AllSeconds >= t2AllSeconds ? true : false;
        }

        public static bool operator <=(Time t1, Time t2)
        {
            int t1AllSeconds = t1.seconds + t1.minutes * 60 + t1.hours * 60 * 60;
            int t2AllSeconds = t2.seconds + t2.minutes * 60 + t2.hours * 60 * 60;
            return t1AllSeconds <= t2AllSeconds ? true : false;
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
            string h = hours >= 10 ? $"{hours}" : $"0{hours}";
            string m = minutes >= 10 ? $"{minutes}" : $"0{minutes}";
            string s = seconds >= 10 ? $"{seconds}" : $"0{seconds}";

            return h + ":" + m + ":" + s;
        }

        public bool isNull()
        {
            int allSeconds = seconds + minutes * 60 + hours * 60 * 60;
            return allSeconds <= 0 ? true : false;
        }


    }
}
