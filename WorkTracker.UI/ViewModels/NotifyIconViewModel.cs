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
        private StatsService statsService;

        public NotifyIconViewModel(StateManager stateManager, StatsService statsService)
        {
            this.stateManager = stateManager;
            this.statsService = statsService;
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
                        statsService.UpdateStats();
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
                            //int i = 0;
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
                return "Break: " + getShortTimeSpanString(statsService.GetRealTimeTodayStats().BreakTime);
            }
        }

        public string WorkTimeText
        {
            get
            {
                return "Work: " + getShortTimeSpanString(statsService.GetRealTimeTodayStats().WorkTime);
            }
        }


        public string TotalTimeText
        {
            get
            {
                return "Total: " + getShortTimeSpanString(statsService.GetRealTimeTodayStats().TotalTime);
            }
        }

        public string OvertimeInfoText
        {
            get
            {
                var globalStats = statsService.GetRealTimeGlobalStats();
                return "Difference: " + getShortTimeSpanString(globalStats.TimeDifference) + " " + (globalStats.TimeType == TimeDifferenceEnum.Overtime ? "overtime" : "undertime");
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
            OnPropertyChanged("OvertimeInfoText");
        }

        private string getShortTimeSpanString(TimeSpan timespan)
        {
            return new TimeSpan(timespan.Hours, timespan.Minutes, timespan.Seconds).ToString();
        }
    }
}
