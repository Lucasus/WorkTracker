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
            var stateChangeRepository = new StateChangeRepository(config);
            var statsCalculator = new StatsCalculator(new DailyStatsRepository(config), stateChangeRepository);
            stateManager = new StateManager(stateChangeRepository, statsCalculator);
            viewModel = new NotifyIconViewModel(stateManager, statsCalculator, new StatsCache(stateManager, statsCalculator, stateChangeRepository));
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
