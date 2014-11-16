using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Business
{
    public class Config
    {
        private string baseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public string ActivityLogsFilePath
        {
            get { return Path.Combine(baseDirectory, "activityLogs.csv"); }
        }

        public string StatsFilePath
        {
            get { return Path.Combine(baseDirectory, "stats.csv"); }
        }
    }
}
