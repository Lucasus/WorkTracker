using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Business
{
    public class BreakState : State
    {
        public override State GetOpposite()
        {
            return new WorkState();
        }

        public override State GetOppositeOrStopped()
        {
            return new StoppedState();
        }

        public override Icon Icon
        {
            get
            {
                return new Icon("Resources/circle-yellow.ico");
            }
        }

        public override string ChangeStateText()
        {
            return "Stop";
        }

        public override StateNamesEnum Name
        {
            get { return StateNamesEnum.Break; }
        }
    }
}
