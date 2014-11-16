using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Business
{
    public abstract class State 
    {
        public abstract State GetOpposite();
        public abstract State GetOppositeOrStopped();
        public abstract Icon Icon { get; }
        public abstract StateNamesEnum Name { get; }
        public abstract string ChangeStateText();
    }
}
