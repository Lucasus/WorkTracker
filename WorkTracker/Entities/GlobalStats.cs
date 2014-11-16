using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Business;

namespace WorkTracker.Entities
{
    public class GlobalStats
    {
        public TimeDifferenceEnum TimeType { get; set; } 
        public TimeSpan TimeDifference { get; set; }
    }
}
