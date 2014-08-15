using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorkTracker.Business;
using WorkTracker.Infrastructure;

namespace WorkTracker.UI
{

    public class NotifyIconViewModel : ObservableViewModel
    {
        private StateManager workStateManager;
        private StatsCalculator statsCalculator;

        public NotifyIconViewModel(StateManager workStateManager, StatsCalculator statsCalculator)
        {
            this.workStateManager = workStateManager;
            this.statsCalculator = statsCalculator;
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

        public ICommand UpdateStatsCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        statsCalculator.UpdateStatsFile();
                    }
                };
            }
        }
     
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                        {
                            Application.Current.Shutdown();
                        }
                };
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
