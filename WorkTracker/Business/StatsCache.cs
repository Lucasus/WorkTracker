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
        private IList<StateChange> stateChanges;
        private StatsCalculator statsCalculator;
        private StateChangeRepository stateChangeRepository;

        public StatsCache(StateManager stateManager, StatsCalculator statsCalculator, StateChangeRepository stateChangeRepository)
        {
            this.stateManager = stateManager;
            this.statsCalculator = statsCalculator;
            this.stateChangeRepository = stateChangeRepository;
            this.stateManager.StateChanged += stateManager_StateChanged;
        }

        public DailyStats GetCurrentStats()
        {
            loadStateChanges();
            return statsCalculator.CalculateDailyStats(stateChanges.Union(new[] { new StateChange() { ChangeDate = DateTime.Now, StateName = "Stopped" } }).ToList());
        }

        void stateManager_StateChanged(object sender, StateChange stateChange)
        {
            stateChanges.Add(stateChange);
        }

        private void loadStateChanges()
        {
            if (stateChanges == null)
            {
                stateChanges = stateChangeRepository.GetForToday();
            }
        }


    }
}
