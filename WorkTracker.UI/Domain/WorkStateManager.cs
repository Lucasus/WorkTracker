using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.UI
{
    public class WorkStateManager
    {
        public WorkState CurrentState { get; set; }

        public event EventHandler StateChanged;
        public void OnStateChanged()
        {
            StateChanged(this, EventArgs.Empty);
        }

        public WorkStateManager()
        {
            CurrentState = WorkState.Stopped;
        }

        public void ChangeWorkState()
        {
            switch (CurrentState)
            {
                case WorkState.Break:
                    CurrentState = WorkState.Work;
                    break;
                case WorkState.Work:
                    CurrentState = WorkState.Break;
                    break;
                case WorkState.Stopped:
                    CurrentState = WorkState.Work;
                    break;
            }
            OnStateChanged();
        }

        public void ChangeStoppedState()
        {
            switch (CurrentState)
            {
                case WorkState.Break:
                    CurrentState = WorkState.Stopped;
                    break;
                case WorkState.Work:
                    CurrentState = WorkState.Stopped;
                    break;
                case WorkState.Stopped:
                    CurrentState = WorkState.Work;
                    break;
            }
            OnStateChanged();
        }
    }
}
