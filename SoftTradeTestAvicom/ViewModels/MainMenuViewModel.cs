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
        readonly INavigationManager _navigationManager;

        public MainMenuViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private MyCommand goManagerList;

        public MyCommand GoManagerList
        {
            get
            {
                return goManagerList ??
                    (goManagerList = new MyCommand(obj =>
                    {
                        _navigationManager.Navigate(NavigationKeys.ManagerListView);
                    }));
            }
        }
    }
}
