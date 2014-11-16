using System;
using System.Linq;
using System.Collections.Generic;
using WorkTracker.Entities;

namespace WorkTracker.Business
{
    public class DailyStatsCalculator
    {
        public DailyStats GetSingleDayStats(IList<StateChange> singleDayStateChanges) 
        {
            TimeSpan workTime = new TimeSpan();
            TimeSpan breakTime = new TimeSpan();

            DateTime? workStart = null;
            DateTime? workEnd = null;

            foreach (var stateChange in singleDayStateChanges)
            {
                if (stateChange.StateName == StateNamesEnum.Stopped)
                {
                    var intervalStartDate = (stateChange.Previous == null || stateChange.Previous.ChangeDate.Date < stateChange.ChangeDate.Date
                            ? stateChange.ChangeDate.Date
                            : stateChange.Previous.ChangeDate);
                    var interval = stateChange.ChangeDate - intervalStartDate;

                    if (stateChange.Previous == null || stateChange.Previous.StateName == StateNamesEnum.Work)
                    {
                        workTime += interval;
                        if (workStart == null)
                        {
                            workStart = intervalStartDate;
                        }
                        if (workEnd == null || workEnd < stateChange.ChangeDate)
                        {
                            workEnd = stateChange.ChangeDate;
                        }
                    }
                    else if (stateChange.Previous.StateName == StateNamesEnum.Break)
                    {
                        breakTime += interval;
                    }
                }
            }

            if (singleDayStateChanges.Count > 0)
            {
                var lastStateChange = singleDayStateChanges.Last();
                var intervalEndDate = lastStateChange.ChangeDate.Date.AddDays(1);
                var interval = intervalEndDate - lastStateChange.ChangeDate;

                if (lastStateChange.StateName == StateNamesEnum.Work)
                {
                    workTime += interval;
                    workEnd = intervalEndDate.AddMinutes(-1); // we still want to have today date in stats
                }
                else if(lastStateChange.StateName == StateNamesEnum.Break)
                {
                    breakTime += interval;
                }
            }

            return new DailyStats() 
            { 
                BreakTime = breakTime, 
                WorkTime = workTime, 
                StatsDate = (singleDayStateChanges.Count >0) ? singleDayStateChanges.First().ChangeDate.Date : DateTime.MinValue,
                WorkStart = workStart ?? DateTime.MinValue,
                WorkEnd = workEnd ?? DateTime.MinValue
            };
        }
    }
}
