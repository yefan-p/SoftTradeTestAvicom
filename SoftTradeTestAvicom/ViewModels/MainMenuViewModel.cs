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
                _navigationManager.Navigate(NavigationKeys.ManagerListView);
            });

        public Command GoProductList =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ProductListView);
            });

        public Command GoClientList =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ClientListView);
            });
    }
}
