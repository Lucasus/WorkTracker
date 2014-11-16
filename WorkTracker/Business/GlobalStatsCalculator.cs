using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Entities;

namespace WorkTracker.Business
{
    public class GlobalStatsCalculator
    {
        public GlobalStats CalculateGlobalStats(IList<DailyStats> dailyStatsList)
        {
            TimeSpan totalExpectedTime = new TimeSpan(0);
            TimeSpan totalWorkedTime = new TimeSpan(0);

            foreach (var dailyStats in dailyStatsList)
            {
                totalExpectedTime += new TimeSpan(8, 0, 0);
                totalWorkedTime += dailyStats.WorkTime;
            }

            return new GlobalStats()
            {
                TimeType = totalWorkedTime > totalExpectedTime ? TimeDifferenceEnum.Overtime : TimeDifferenceEnum.Undertime,
                TimeDifference = (totalWorkedTime - totalExpectedTime).Duration()
            };
        }
    }
}
