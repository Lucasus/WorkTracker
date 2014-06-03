using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorkTracker.UI.Utilities;

namespace WorkTracker.UI
{

    public class NotifyIconViewModel : INotifyPropertyChanged
    {
        private IconManager iconManager;
        private WorkStateManager workStateManager;

        public NotifyIconViewModel(IconManager iconManager, WorkStateManager workStateManager)
        {
            this.iconManager = iconManager;
            this.workStateManager = workStateManager;
            iconManager.IconChanged += iconManager_IconChanged;
        }

        public ICommand SwitchWorkModeCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        workStateManager.ChangeWorkState();
                    }
                };
            }
        }

        public ICommand SwitchStoppedModeCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        workStateManager.ChangeStoppedState();
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
                return iconManager.CurrentIcon.Path;
            }
        }

        private void iconManager_IconChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("IconPath");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
