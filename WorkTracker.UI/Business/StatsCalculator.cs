using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Entities;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StatsCalculator
    {
        private DailyStatsRepository dailyStatsRepository;
        private StateChangeRepository stateChangeRepository;

        public StatsCalculator(DailyStatsRepository dailyStatsRepository, StateChangeRepository stateChangeRepository) 
        {
            this.dailyStatsRepository = dailyStatsRepository;
            this.stateChangeRepository = stateChangeRepository;
        }

        public void UpdateStatsFile()
        {
            var dailyStatsForToday = calculateStats(stateChangeRepository.GetForToday());
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

        private DailyStats calculateStats(IList<StateChange> stateChanges) 
        {
            TimeSpan workTime = new TimeSpan();
            TimeSpan breakTime = new TimeSpan();
            for (int i = 1; i < stateChanges.Count; ++i)
            {
                var previousState = stateChanges[i - 1];
                var currentState = stateChanges[i];
                if (previousState.StateName == "Work")
                {
                    workTime += currentState.ChangeDate - previousState.ChangeDate;
                }
                if (previousState.StateName == "Break")
                {
                    breakTime += currentState.ChangeDate - previousState.ChangeDate;
                }
            }

            return new DailyStats() { BreakTime = breakTime, WorkTime = workTime, StatsDate = stateChanges[0].ChangeDate.Date };
        }
    }
}
