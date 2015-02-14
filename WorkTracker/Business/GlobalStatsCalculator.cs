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
            // Assuming working 8 hours per day
            var totalExpectedTime = dailyStatsList.Select(x => new TimeSpan(8, 0, 0)).Aggregate((x, y) => x + y);
            var totalWorkedTime = dailyStatsList.Select(x => x.WorkTime).Aggregate((x, y) => x + y);

            return new GlobalStats()
            {
                TimeType = totalWorkedTime > totalExpectedTime ? TimeDifferenceEnum.Overtime : TimeDifferenceEnum.Undertime,
                TimeDifference = (totalWorkedTime - totalExpectedTime).Duration()
            };
        }
    }
}
