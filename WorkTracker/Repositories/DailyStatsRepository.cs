using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Business;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;

namespace WorkTracker.Repositories
{
    public class DailyStatsRepository
    {
        private Config config;
        private string versionNumber = "V1.0";

        public DailyStatsRepository(Config config)
        {
            this.config = config;
        }

        public IList<DailyStats> GetAll()
        {
            if (!File.Exists(config.StatsFilePath))
            {
                using (File.Create(config.StatsFilePath)) { }
            }
            return File.ReadAllLines(config.StatsFilePath).Select(x => fromRow(x)).ToList();
        }

        public void ReplaceWith(IList<DailyStats> allStats)
        {
            File.WriteAllLines(config.StatsFilePath, allStats.Select(x => toRow(x)).ToArray());
        }

        private string toRow(DailyStats entity)
        {
            return String.Join(",", versionNumber, entity.StatsDate.ToShortDateString(), entity.WorkTime, entity.BreakTime, entity.TotalTime);
        }

        private DailyStats fromRow(string row)
        {
            var values = row.Split(',');
            return new DailyStats()
            {
                StatsDate = DateTime.Parse(values[1]),
                WorkTime = TimeSpan.Parse(values[2]),
                BreakTime = TimeSpan.Parse(values[3])
            };
        }


    }
}
