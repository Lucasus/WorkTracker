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
    public class StatsCache
    {
        private StateManager stateManager;
        private StatsManager statsManager;
        private IList<StateChange> todayStateChanges;
        private DailyStatsCalculator statsCalculator;
        private GlobalStatsCalculator globalStatsCalculator;
        private IStateChangeRepository stateChangeRepository;
        private ITimeProvider timeProvider;

        public StatsCache(StateManager stateManager, StatsManager statsManager, DailyStatsCalculator statsCalculator, GlobalStatsCalculator globalStatsCalculator, 
            IStateChangeRepository stateChangeRepository, ITimeProvider timeProvider)
        {
            this.stateManager = stateManager;
            this.statsManager = statsManager;
            this.statsCalculator = statsCalculator;
            this.stateChangeRepository = stateChangeRepository;
            this.globalStatsCalculator = globalStatsCalculator;
            this.timeProvider = timeProvider;
            this.stateManager.StateChanged += stateManager_StateChanged;
        }

        public DailyStats GetCurrentTodayStats()
        {
            return statsCalculator.GetSingleDayStats(getStateChangesForCalculation());
        }

        public GlobalStats GetGlobalStats()
        {
            return globalStatsCalculator.CalculateGlobalStats(statsManager.GetCurrentDailyStats(getStateChangesForCalculation()));
        }

        void stateManager_StateChanged(object sender, StateChange stateChange)
        {
            todayStateChanges.Add(stateChange);
        }

        private IList<StateChange> getStateChangesForCalculation()
        {
            loadStateChanges();
            var lastStateChange = stateChangeRepository.GetLast();
            var stateChangesForCalculation = todayStateChanges;
            if (lastStateChange != null && (lastStateChange.StateName == StateNamesEnum.Work || lastStateChange.StateName == StateNamesEnum.Break))
            {
                stateChangesForCalculation = stateChangesForCalculation.Union(new[] { new StateChange(StateNamesEnum.Stopped, timeProvider.CurrentDate, lastStateChange) }).ToList();
            }
            return stateChangesForCalculation;
        }

        private void loadStateChanges()
        {
            if (todayStateChanges == null)
            {
                todayStateChanges = stateChangeRepository.GetByDate(timeProvider.CurrentDate);
            }
        }
    }
}
