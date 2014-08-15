using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorkTracker.Business;
using WorkTracker.Entities;
using WorkTracker.Infrastructure;

namespace WorkTracker.UI
{

    public class NotifyIconViewModel : ObservableViewModel
    {
        private StateManager stateManager;
        private StatsCalculator statsCalculator;
        private StatsCache statsCache;

        public NotifyIconViewModel(StateManager stateManager, StatsCalculator statsCalculator, StatsCache statsCache)
        {
            this.stateManager = stateManager;
            this.statsCalculator = statsCalculator;
            this.statsCache = statsCache;
            stateManager.StateChanged += stateManager_StateChanged;
        }

        public ICommand SwitchWorkModeCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        stateManager.ChangeWorkOrBreakToOpposite();
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
                        stateManager.ChangeStartOrStopToOpposite();
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

        public ICommand TrayToolTipOpen
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                        {
                            int i = 0;
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
                return stateManager.CurrentState.Icon.Path;
            }
        }

        public string BreakTimeText
        {
            get
            {
                return "Break: " + getShortTimeSpanString(statsCache.GetCurrentStats().BreakTime);
            }
        }

        public string WorkTimeText
        {
            get
            {
                return "Work: " + getShortTimeSpanString(statsCache.GetCurrentStats().WorkTime);
            }
        }


        public string TotalTimeText
        {
            get
            {
                return "Total: " + getShortTimeSpanString(statsCache.GetCurrentStats().TotalTime);
            }
        }

        public string StartStopMenuHeader
        {
            get
            {
                return stateManager.CurrentState.ChangeStateText();
            }
        }

        void stateManager_StateChanged(object sender, StateChange newState)
        {
            OnPropertyChanged("IconPath");
            OnPropertyChanged("StartStopMenuHeader");
            StatsChanged();
        }

        public void StatsChanged()
        {
            OnPropertyChanged("TotalTimeText");
            OnPropertyChanged("WorkTimeText");
            OnPropertyChanged("BreakTimeText");
        }

        private string getShortTimeSpanString(TimeSpan timespan)
        {
            return new TimeSpan(timespan.Hours, timespan.Minutes, timespan.Seconds).ToString();
        }
    }
}
