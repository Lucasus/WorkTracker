using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;
using WorkTracker.Repositories;

namespace WorkTracker.Business
{
    public class StateManager
    {        
        private IStateChangeRepository stateChangeRepository;
        private StatsService statsManager;
        private ITimeProvider timeProvider;

        public State CurrentState { get; private set; }

        public event EventHandler<StateChange> StateChanged;
        public void OnStateChanged(StateChange stateChange)
        {
            if (StateChanged != null)
            {
                StateChanged(this, stateChange);
            }
        }

        public StateManager(IStateChangeRepository stateChangeRepository, StatsService statsManager, ITimeProvider timeProvider)
        {
            this.stateChangeRepository = stateChangeRepository;
            this.statsManager = statsManager;
            this.timeProvider = timeProvider;
        }

        public void StopWork()
        {
            changeState(new StoppedState());
            statsManager.UpdateStats();
        }

        public void StartWork()
        {
            changeState(new WorkState());
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
                var stateChange = new StateChange(newState.Name, timeProvider.CurrentDate, stateChangeRepository.GetLast());
                stateChangeRepository.Save(stateChange);
                OnStateChanged(stateChange);
            }
        }
    }
}
