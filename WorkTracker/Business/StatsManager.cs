using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Entities;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StatsManager
    {
        private IDailyStatsRepository dailyStatsRepository;
        private IStateChangeRepository stateChangeRepository;
        private DailyStatsCalculator calculator;

        public StatsManager(IDailyStatsRepository dailyStatsRepository, IStateChangeRepository stateChangeRepository, DailyStatsCalculator calculator) 
        {
            this.dailyStatsRepository = dailyStatsRepository;
            this.stateChangeRepository = stateChangeRepository;
            this.calculator = calculator;
        }

        public IList<DailyStats> GetCurrentDailyStats(IList<StateChange> todayStateChanges)
        {
            var dailyStatsForToday = calculator.GetSingleDayStats(todayStateChanges);
            var allDailyStats = dailyStatsRepository.GetAll();
            if (allDailyStats.Count == 0 || allDailyStats.Last().StatsDate.Date != DateTime.Now.Date)
            {
                allDailyStats.Add(dailyStatsForToday);
            }
            else
            {
                allDailyStats[allDailyStats.Count - 1] = dailyStatsForToday;
            }
            return allDailyStats;
        }

        public void UpdateStats()
        {
            var currentDailyStatsList = GetCurrentDailyStats(stateChangeRepository.GetByDate(DateTime.Now));
            dailyStatsRepository.ReplaceWith(currentDailyStatsList);
        }
    }
}
