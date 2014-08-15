using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WorkTracker.Entities;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StateManager
    {        
        private StateChangeRepository stateChangeRepository;

        public State CurrentState { get; set; }

        public event EventHandler StateChanged;
        public void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        public StateManager(StateChangeRepository stateChangeRepository)
        {
            this.stateChangeRepository = stateChangeRepository;
            changeState(new WorkState());
        }

        public void StopWork()
        {
            changeState(new StoppedState());
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
            if (CurrentState == null || newState.Name != CurrentState.Name)
            {
                CurrentState = newState;
                stateChangeRepository.Save(new StateChange { StateName = newState.Name, ChangeDate = DateTime.Now });
                OnStateChanged();
            }
        }
    }
}
