using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StatsManager
    {
        private IDailyStatsRepository dailyStatsRepository;
        private IStateChangeRepository stateChangeRepository;
        private StatsCalculator calculator;

        public StatsManager(IDailyStatsRepository dailyStatsRepository, IStateChangeRepository stateChangeRepository, StatsCalculator calculator) 
        {
            this.dailyStatsRepository = dailyStatsRepository;
            this.stateChangeRepository = stateChangeRepository;
            this.calculator = calculator;
        }

        public void UpdateStats()
        {
            var dailyStatsForToday = calculator.GetSingleDayStats(stateChangeRepository.GetByDate(DateTime.Now));
            var allDailyStats = dailyStatsRepository.GetAll();
            if (allDailyStats.Count == 0 || allDailyStats.Last().StatsDate.Date != DateTime.Now.Date)
            {
                allDailyStats.Add(dailyStatsForToday);
            }
            else
            {
                allDailyStats[allDailyStats.Count - 1] = dailyStatsForToday;
            }
            dailyStatsRepository.ReplaceWith(allDailyStats);
        }
    }
}
