using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WorkTracker.Domain
{
    public class WorkStateManager
    {
        private StateLogger stateLogger;

        public State CurrentState { get; set; }

        public event EventHandler StateChanged;
        public void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        public WorkStateManager(StateLogger stateLogger)
        {
            this.stateLogger = stateLogger;
            changeState(new Stopped());
        }

        public void ChangeWorkOrBreakToOpposite()
        {
            changeState(CurrentState.GetOpposite());
        }

        public void ChangeStartOrStopToOpposite()
        {
            changeState(CurrentState.GetOppositeOrStopped());
        }

        private void changeState(State newState)
        {
            CurrentState = newState;
            stateLogger.LogState(newState);
            OnStateChanged();
        }
    }
}
