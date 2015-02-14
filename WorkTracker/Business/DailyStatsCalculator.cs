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
            if (singleDayStateChanges.Count > 0)
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
                        var stateDuration = stateChange.ChangeDate - intervalStartDate;

                        if (stateChange.Previous == null || stateChange.Previous.StateName == StateNamesEnum.Work)
                        {
                            workTime += stateDuration;
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
                            breakTime += stateDuration;
                        }
                    }
                }

                // Last state duration is trickier, because state may change in the next day or not at all
                var lastStateChange = singleDayStateChanges.Last();
                if (lastStateChange.StateName != StateNamesEnum.Stopped)
                {
                    var endOfDay = lastStateChange.ChangeDate.Date.AddDays(1);
                    var lastStateDuration = endOfDay - lastStateChange.ChangeDate;

                    if (lastStateChange.StateName == StateNamesEnum.Work)
                    {
                        workTime += lastStateDuration;
                        workEnd = endOfDay.AddMinutes(-1); // we still want to have today date in stats
                    }
                    else if (lastStateChange.StateName == StateNamesEnum.Break)
                    {
                        breakTime += lastStateDuration;
                    }
                }

                return new DailyStats()
                {
                    BreakTime = breakTime,
                    WorkTime = workTime,
                    StatsDate = singleDayStateChanges.First().ChangeDate.Date,
                    WorkStart = workStart ?? DateTime.MinValue,
                    WorkEnd = workEnd ?? DateTime.MinValue
                };
            }

            return new DailyStats()
            {
                BreakTime = new TimeSpan(),
                WorkTime = new TimeSpan(),
                StatsDate = DateTime.MinValue,
                WorkStart = DateTime.MinValue,
                WorkEnd = DateTime.MinValue
            };
        }
    }
}
