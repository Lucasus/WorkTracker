using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StatsManager
    {
        private IDailyStatsRepository dailyStatsRepository;
        private IStateChangeRepository stateChangeRepository;
        private DailyStatsCalculator calculator;
        private ITimeProvider timeProvider;

        public StatsManager(IDailyStatsRepository dailyStatsRepository, IStateChangeRepository stateChangeRepository,
            DailyStatsCalculator calculator, ITimeProvider timeProvider) 
        {
            this.dailyStatsRepository = dailyStatsRepository;
            this.stateChangeRepository = stateChangeRepository;
            this.calculator = calculator;
            this.timeProvider = timeProvider;
        }

        public IList<DailyStats> GetCurrentDailyStats(IList<StateChange> todayStateChanges)
        {
            var dailyStatsForToday = calculator.GetSingleDayStats(todayStateChanges);
            var allDailyStats = dailyStatsRepository.GetAll();
            if (allDailyStats.Count == 0 || allDailyStats.Last().StatsDate.Date != timeProvider.CurrentDate)
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
            var currentDailyStatsList = GetCurrentDailyStats(stateChangeRepository.GetByDate(timeProvider.CurrentDate));
            dailyStatsRepository.ReplaceWith(currentDailyStatsList);
        }
    }
}
