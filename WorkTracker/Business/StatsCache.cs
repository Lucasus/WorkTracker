using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Entities;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StatsCache
    {
        private StateManager stateManager;
        private IList<StateChange> todayStateChanges;
        private StatsCalculator statsCalculator;
        private IStateChangeRepository stateChangeRepository;

        public StatsCache(StateManager stateManager, StatsCalculator statsCalculator, IStateChangeRepository stateChangeRepository)
        {
            this.stateManager = stateManager;
            this.statsCalculator = statsCalculator;
            this.stateChangeRepository = stateChangeRepository;
            this.stateManager.StateChanged += stateManager_StateChanged;
        }

        public DailyStats GetCurrentStats()
        {
            loadStateChanges();
            var lastStateChange = stateChangeRepository.GetLast();
            var stateChangesForCalculation = todayStateChanges;
            if (lastStateChange.StateName == StateNamesEnum.Work || lastStateChange.StateName == StateNamesEnum.Break)
            {
                stateChangesForCalculation = stateChangesForCalculation.Union(new[] { new StateChange(StateNamesEnum.Stopped, DateTime.Now, lastStateChange) }).ToList();
            }
            return statsCalculator.GetSingleDayStats(stateChangesForCalculation);
        }

        void stateManager_StateChanged(object sender, StateChange stateChange)
        {
            todayStateChanges.Add(stateChange);
        }

        private void loadStateChanges()
        {
            if (todayStateChanges == null)
            {
                todayStateChanges = stateChangeRepository.GetByDate(DateTime.Now);
            }
        }


    }
}
