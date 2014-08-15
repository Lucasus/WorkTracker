using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Business
{
    public class StoppedState : State
    {
        public override State GetOpposite()
        {
            return new WorkState();
        }

        public override State GetOppositeOrStopped()
        {
            return new WorkState();
        }

        public override Icon Icon
        {
            get
            {
                return new Icon("/circle-gray.ico");
            }
        }

        public override string ChangeStateText()
        {
            return "Start";
        }

        public override string Name
        {
            get { return "Stopped"; }
        }
    }
}
