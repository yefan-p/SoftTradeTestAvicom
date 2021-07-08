using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Utils;

namespace SoftTradeTestAvicom.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly INavigationManager _navigationManager;

        public MainMenuViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public Command GoManagerList =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ManagerListView,
                    NavigationFrom = NavigationKeys.MainMenuView
                };
                _navigationManager.Navigate(input);
            });

        public Command GoProductList =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ProductListView,
                    NavigationFrom = NavigationKeys.MainMenuView
                };
                _navigationManager.Navigate(input);
            });

        public Command GoClientList =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ClientListView,
                    NavigationFrom = NavigationKeys.MainMenuView
                };
                _navigationManager.Navigate(input);
            });
    }
}
