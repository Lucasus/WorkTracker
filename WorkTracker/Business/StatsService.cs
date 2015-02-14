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
    public class StatsService
    {
        private IDailyStatsRepository dailyStatsRepository;
        private IStateChangeRepository stateChangeRepository;
        private DailyStatsCalculator statsCalculator;
        private GlobalStatsCalculator globalStatsCalculator;
        private ITimeProvider timeProvider;

        public StatsService(IDailyStatsRepository dailyStatsRepository, IStateChangeRepository stateChangeRepository,
            DailyStatsCalculator statsCalculator, GlobalStatsCalculator globalStatsCalculator, ITimeProvider timeProvider) 
        {
            this.dailyStatsRepository = dailyStatsRepository;
            this.stateChangeRepository = stateChangeRepository;
            this.statsCalculator = statsCalculator;
            this.globalStatsCalculator = globalStatsCalculator;
            this.timeProvider = timeProvider;
        }

        public DailyStats GetRealTimeTodayStats()
        {
            return statsCalculator.GetSingleDayStats(getRealTimeStateChanges());
        }

        public GlobalStats GetRealTimeGlobalStats()
        {
            return globalStatsCalculator.CalculateGlobalStats(getCurrentDailyStats(getRealTimeStateChanges()));
        }

        public void UpdateStats()
        {
            dailyStatsRepository.ReplaceWith(getCurrentDailyStats(stateChangeRepository.GetByDate(timeProvider.CurrentDate)));
        }

        private IList<StateChange> getRealTimeStateChanges()
        {
            var stateChangesForCalculation = stateChangeRepository.GetByDate(timeProvider.CurrentDate);
            var lastStateChange = stateChangesForCalculation.Last();
            if (lastStateChange != null && (lastStateChange.StateName == StateNamesEnum.Work || lastStateChange.StateName == StateNamesEnum.Break))
            {
                stateChangesForCalculation = stateChangesForCalculation.Union(new[] { new StateChange(StateNamesEnum.Stopped, timeProvider.CurrentDate, lastStateChange) }).ToList();
            }
            return stateChangesForCalculation;
        }

        private IList<DailyStats> getCurrentDailyStats(IList<StateChange> todayStateChanges)
        {
            var dailyStatsForToday = statsCalculator.GetSingleDayStats(todayStateChanges);
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
    }
}
