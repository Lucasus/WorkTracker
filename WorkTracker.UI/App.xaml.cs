using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkTracker.Business;
using WorkTracker.Infrastructure;
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
        private NotifyIconViewModel viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            var config = new Config();
            var stateChangeRepository = new StateChangeRepository(new FileDataProvider(config.ActivityLogsFilePath));
            var statsCalculator = new DailyStatsCalculator();
            var globalStatsCalculator = new GlobalStatsCalculator();
            var statsManager = new StatsManager(new DailyStatsRepository(new FileDataProvider(config.StatsFilePath)), stateChangeRepository, statsCalculator);
            stateManager = new StateManager(stateChangeRepository, statsManager);
            viewModel = new NotifyIconViewModel(stateManager, statsManager, new StatsCache(stateManager, statsManager, statsCalculator, globalStatsCalculator, stateChangeRepository));
            notifyIcon.DataContext = viewModel;
            notifyIcon.TrayToolTipOpen +=notifyIcon_TrayToolTipOpen; 
            stateManager.StartWork();
        }

        void notifyIcon_TrayToolTipOpen(object sender, RoutedEventArgs e)
        {
            viewModel.StatsChanged();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            stateManager.StopWork();
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
