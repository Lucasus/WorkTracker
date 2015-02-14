using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Business;
using WorkTracker.Entities;

namespace WorkTracker.Tests.Infrastructure
{
    public static class DataBuilder
    {
        public static IList<StateChange> StateChangesCollection(params Tuple<StateNamesEnum, string>[] stateChanges)
        {
            var stateChangesCollection = new List<StateChange>();
            for(int i = 0; i < stateChanges.Length; ++i)
            {
                stateChangesCollection.Add(new StateChange(stateChanges[i].Item1, stateChanges[i].Item2.Date(), i > 0 ? stateChangesCollection[i - 1] : null));
            }
            return stateChangesCollection;
        }
    }
}
