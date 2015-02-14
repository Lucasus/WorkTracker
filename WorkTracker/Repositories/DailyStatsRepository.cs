using System;
using System.Collections.Generic;
using System.Linq;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;

namespace WorkTracker.Repositories
{
    public class DailyStatsRepository : IDailyStatsRepository
    {
        private IStringDataProvider dataProvider;
        private string versionNumber = "V1.1";

        public DailyStatsRepository(IStringDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public IList<DailyStats> GetAll()
        {
            return dataProvider.GetAll().Select(x => fromRow(x)).ToList();
        }

        public void ReplaceWith(IList<DailyStats> allStats)
        {
            dataProvider.OverrideWith(allStats.Select(x => toRow(x)).ToList());
        }

        private string toRow(DailyStats entity)
        {
            return String.Join(",", versionNumber, 
                entity.StatsDate.ToShortDateString(), 
                entity.WorkStart.ToShortTimeString(), 
                entity.WorkEnd.ToShortTimeString(), 
                entity.WorkTime, 
                entity.BreakTime, 
                entity.TotalTime);
        }

        private DailyStats fromRow(string row)
        {
            var values = row.Split(',');
            if (values[0] == versionNumber)
            {
                return new DailyStats()
                {
                    StatsDate = DateTime.Parse(values[1]),
                    WorkStart = DateTime.Parse(values[2]),
                    WorkEnd = DateTime.Parse(values[3]),
                    WorkTime = TimeSpan.Parse(values[4]),
                    BreakTime = TimeSpan.Parse(values[5])
                };
            }
            else
            {
                return new DailyStats()
                {
                    StatsDate = DateTime.Parse(values[1]),
                    WorkTime = TimeSpan.Parse(values[2]),
                    BreakTime = TimeSpan.Parse(values[3])
                };
            }
        }
    }
}
