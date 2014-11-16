using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Entities
{
    public class DailyStats
    {
        public DateTime StatsDate { get; set; }
        public TimeSpan WorkTime { get; set; }
        public TimeSpan BreakTime { get; set; }

        public DateTime WorkStart { get; set; }
        public DateTime WorkEnd { get; set; }

        public TimeSpan TotalTime
        {
            get
            {
                return WorkTime + BreakTime;
            }
        }
    }
}
