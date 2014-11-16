using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Business;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;

namespace WorkTracker.Repositories
{
    public class StateChangeRepository
    {
        private Config config;
        private string versionNumber = "V1.0";
        private bool getForToday_isDirty = true;
        private IList<StateChange> getForToday_cached;

        public StateChangeRepository(Config config)
        {
            this.config = config;
        }

        public IList<StateChange> GetForToday()
        {
            if (getForToday_isDirty)
            {
                getForToday_isDirty = false;
                getForToday_cached = File.ReadAllLines(config.ActivityLogsFilePath)
                    .Select(x => fromRow(x))
                    .Where(x => x.ChangeDate.Date == DateTime.Now.Date)
                    .ToList();
            }
            return getForToday_cached;
        }

        public void Save(StateChange stateChange)
        {
            getForToday_isDirty = true;
            toRow(stateChange).AppendToFile(config.ActivityLogsFilePath);
        }

        private string toRow(StateChange stateChange)
        {
            return String.Join(",", versionNumber, stateChange.StateName, stateChange.ChangeDate);
        }

        private StateChange fromRow(string row)
        {
            var values = row.Split(',');
            return new StateChange()
            {
                StateName = values[1],
                ChangeDate = DateTime.Parse(values[2])
            };
        }

    }
}
