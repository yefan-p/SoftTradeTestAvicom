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
using SoftTradeTestAvicom.Models;

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
            var managerEditViewModel = _serviceProvider.GetService<ManagerEditViewModel>();

            navigator.Register<MainMenuViewModel, MainMenuView>(mainMenuViewModel, NavigationKeys.MainMenuView);
            navigator.Register<ManagerListViewModel, ManagerListView>(managerListViewModel, NavigationKeys.ManagerListView);
            navigator.Register<ManagerEditViewModel, ManagerEditView>(managerEditViewModel, NavigationKeys.ManagerEditView);

            navigator.Navigate(NavigationKeys.MainMenuView);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var window = new MainWindow();
            var mainNavManager = new NavigationManager(Dispatcher, window.FrameContent);

            services.AddTransient<INavigationManager>(p =>
            {
                return mainNavManager;
            });

            services.AddDbContext<SoftTradeDbContext>();

            services.AddTransient<MainMenuViewModel, MainMenuViewModel>();
            services.AddTransient<ManagerListViewModel, ManagerListViewModel>();
            services.AddTransient<ManagerEditViewModel, ManagerEditViewModel>();

            window.Show();
        }
    }
}
