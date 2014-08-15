using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WorkTracker.Business;
using WorkTracker.Repositories;

namespace WorkTracker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        private StateManager stateManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            var config = new Config();
            var changeStateRepository = new StateChangeRepository(config);
            var statsCalculator = new StatsCalculator(new DailyStatsRepository(config), changeStateRepository);
            stateManager = new StateManager(changeStateRepository, statsCalculator);
            notifyIcon.DataContext = new NotifyIconViewModel(stateManager, statsCalculator);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            stateManager.StopWork();
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
