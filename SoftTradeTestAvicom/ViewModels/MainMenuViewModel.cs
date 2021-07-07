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

        private Command _goManagerList;

        public Command GoManagerList
        {
            get
            {
                return _goManagerList ??
                    (_goManagerList = new Command(obj =>
                    {
                        _navigationManager.Navigate(NavigationKeys.ManagerListView);
                    }));
            }
        }
    }
}
