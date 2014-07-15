using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorkTracker.Domain;
using WorkTracker.UI.Infrastructure;
using WorkTracker.UI.Utilities;

namespace WorkTracker.UI
{

    public class NotifyIconViewModel : ObservableViewModel
    {
        private WorkStateManager workStateManager;

        public NotifyIconViewModel(WorkStateManager workStateManager)
        {
            this.workStateManager = workStateManager;
            workStateManager.StateChanged += workStateManager_StateChanged;
        }

        public ICommand SwitchWorkModeCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        workStateManager.ChangeWorkOrBreakToOpposite();
                    }
                };
            }
        }

        public ICommand StartOrStopCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        workStateManager.ChangeStartOrStopToOpposite();
                    }
                };
            }
        }
     
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }

        public string IconPath
        {
            get
            {
                return workStateManager.CurrentState.Icon.Path;
            }
        }

        public string StartStopMenuHeader
        {
            get
            {
                return workStateManager.CurrentState.ChangeStateText();
            }
        }

        void workStateManager_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("IconPath");
            OnPropertyChanged("StartStopMenuHeader");
        }
    }
}
