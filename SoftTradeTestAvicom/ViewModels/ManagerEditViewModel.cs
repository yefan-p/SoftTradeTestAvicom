using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Utils;
using SoftTradeTestAvicom.Models;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ManagerEditViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationManager _navigationManager;

        private readonly SoftTradeDbContext _db;

        private Manager _manager;

        private bool _addNew;

        public ManagerEditViewModel(INavigationManager navigationManager, SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;
        }

        public Command Cancel =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ManagerListView);
            });

        public Command Ok =>
            new(obj =>
            {
                if (_addNew)
                {
                    _ = _db.Managers.Add(Manager);
                    _ = _db.SaveChanges();
                    _navigationManager.Navigate(NavigationKeys.ManagerListView, Manager);
                }
                else
                {
                    _ = _db.Managers.Update(Manager);
                    _ = _db.SaveChanges();
                    _navigationManager.Navigate(NavigationKeys.ManagerListView, Manager);
                }
            });

        public Manager Manager
        {
            get => _manager;
            set { _manager = value; OnPropertyChanged(); }
        }

        public void OnNavigatingTo(object arg)
        {
            if (arg is Manager manager)
            {
                Manager = manager;
                _addNew = false;
            }
            else
            {
                _addNew = true;
                Manager = new Manager();
            }
        }

        public void OnNavigatingFrom()
        {
            Manager = null;
        }
    }
}
