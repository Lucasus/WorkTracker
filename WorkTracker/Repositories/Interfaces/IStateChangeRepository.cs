using System;
using System.Collections.Generic;
using WorkTracker.Business;
using WorkTracker.Entities;

namespace WorkTracker.Repositories
{
    public interface IStateChangeRepository
    {
        IList<StateChange> GetByDate(DateTime date);
        void Save(StateChange stateChange);
        StateChange GetLast();
    }
}
