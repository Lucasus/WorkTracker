using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
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
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            // Poor man's DI:
            var config = new Config();
            var stateChangeRepository = new StateChangeRepository(new FileDataProvider(config.ActivityLogsFilePath));
            var statsCalculator = new DailyStatsCalculator();
            var globalStatsCalculator = new GlobalStatsCalculator();
            var timeProvider = new TimeProvider();
            var statsService = new StatsService(new DailyStatsRepository(new FileDataProvider(config.StatsFilePath)), stateChangeRepository, statsCalculator, globalStatsCalculator, timeProvider);
            stateManager = new StateManager(stateChangeRepository, statsService, timeProvider);
            viewModel = new NotifyIconViewModel(stateManager, statsService);

            notifyIcon.DataContext = viewModel;
            notifyIcon.TrayToolTipOpen += notifyIcon_TrayToolTipOpen;
            stateManager.StartWork();
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if(e.Mode == PowerModes.Suspend)
            {
                stateManager.StopWork();
            }
            else if(e.Mode == PowerModes.Resume)
            {
                stateManager.StartWork();
            }
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
