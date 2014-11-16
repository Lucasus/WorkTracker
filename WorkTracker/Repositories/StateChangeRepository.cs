using System;
using System.Collections.Generic;
using System.Linq;
using WorkTracker.Business;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;

namespace WorkTracker.Repositories
{
    public class StateChangeRepository : IStateChangeRepository
    {
        private IStringDataProvider dataProvider;
        private string versionNumber = "V1.0";

        private bool getAll_isDirty = true;
        private IList<StateChange> getAll_cached;

        public StateChangeRepository(IStringDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public IList<StateChange> GetByDate(DateTime date)
        {
            return GetAll()
                .Where(x => x.ChangeDate.Date == date.Date)
                .ToList();
        }

        public IList<StateChange> GetAll()
        {
            if (getAll_isDirty)
            {
                getAll_isDirty = false;
                getAll_cached = new List<StateChange>();
                StateChange state = null;
                foreach(var row in dataProvider.GetAll())
                {
                    state = fromRow(row, state);
                    getAll_cached.Add(state);
                }
            }
            return getAll_cached;
        }

        public StateChange GetLast()
        {
            if (GetAll().Count == 0)
            {
                return null;
            }
            return GetAll().Last();
        }

        public void Save(StateChange stateChange)
        {
            getAll_isDirty = true;
            dataProvider.AddRecord(toStringRow(stateChange));
        }

        private string toStringRow(StateChange stateChange)
        {
            return String.Join(",", versionNumber, stateChange.StateName, stateChange.ChangeDate);
        }

        private StateChange fromRow(string row, StateChange previous)
        {
            var values = row.Split(',');
            return new StateChange((StateNamesEnum)Enum.Parse(typeof(StateNamesEnum), values[1]), DateTime.Parse(values[2]), previous);
        }

    }
}
