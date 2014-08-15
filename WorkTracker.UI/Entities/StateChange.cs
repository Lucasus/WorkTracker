using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Entities
{
    public class StateChange
    {
        public string StateName { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
