using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Business;

namespace WorkTracker.Entities
{
    public class StateChange
    {
        public StateNamesEnum StateName { get; private set; }
        public DateTime ChangeDate { get; private set; }
        public StateChange Previous { get; private set; }

        public StateChange(StateNamesEnum stateName, DateTime changeDate, StateChange previous)
        {
            this.StateName = stateName;
            this.ChangeDate = changeDate;
            this.Previous = previous;
        }
    }
}
