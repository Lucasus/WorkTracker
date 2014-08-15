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

        public StateChangeRepository(Config config)
        {
            this.config = config;
        }

        public IList<StateChange> GetForToday()
        {
            return File.ReadAllLines(config.ActivityLogsFilePath)
                .Select(x => fromRow(x))
                .Where(x => x.ChangeDate.Date == DateTime.Now.Date)
                .ToList();
        }

        public void Save(StateChange stateChange)
        {
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
