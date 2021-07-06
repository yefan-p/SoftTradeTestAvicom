using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using SoftTradeTestAvicom.Views;
using SoftTradeTestAvicom.ViewModels;
using SoftTradeTestAvicom.Utils;

namespace SoftTradeTestAvicom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var navigator = _serviceProvider.GetService<INavigationManager>();
            var mainMenuViewModel = _serviceProvider.GetService<MainMenuViewModel>();
            var managerListViewModel = _serviceProvider.GetService<ManagerListViewModel>();

            navigator.Register<MainMenuViewModel, MainMenuView>(mainMenuViewModel, NavigationKeys.MainMenuView);
            navigator.Register<ManagerListViewModel, ManagerListView>(managerListViewModel, NavigationKeys.ManagerListView);

            navigator.Navigate(NavigationKeys.MainMenuView);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var window = new MainWindow();
            var mainNavManager = new NavigationManager(Dispatcher, window.FrameContent);

            services.AddSingleton<INavigationManager>(mainNavManager);

            services.AddTransient<MainMenuViewModel, MainMenuViewModel>();
            services.AddTransient<ManagerListViewModel, ManagerListViewModel>();

            window.Show();
        }
    }
}
