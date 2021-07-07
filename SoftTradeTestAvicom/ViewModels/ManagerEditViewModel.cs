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

        public ManagerEditViewModel(INavigationManager navigationManager, SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;
        }

        private Command _cancel;

        public Command Cancel
        {
            get
            {
                return _cancel ??
                    (_cancel = new Command(obj =>
                    {
                        _navigationManager.Navigate(NavigationKeys.ManagerListView);
                    }));
            }
        }

        private Command _ok;

        public Command Ok
        {
            get
            {
                return _ok ??
                    (_ok = new Command(obj =>
                    {
                        if (_manager == null)
                        {
                            var manager = new Manager
                            {
                                Created = DateTime.Now,
                                Name = _name
                            };
                            _db.Managers.Add(manager);
                            _db.SaveChanges();
                            _navigationManager.Navigate(NavigationKeys.ManagerListView, manager);
                            Name = String.Empty;
                        }
                        else
                        {
                            _manager.Name = Name;
                            _db.Managers.Update(_manager);
                            _db.SaveChanges();
                            _navigationManager.Navigate(NavigationKeys.ManagerListView, _manager);
                            Name = String.Empty;
                        }
                        
                    }));
            }
        }

        private Manager _manager;

        private string _name;

        public string Name 
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public void OnNavigatingTo(object arg)
        {
            if (arg != null)
            {
                _manager = (Manager)arg;
                Name = _manager.Name;
            };
        }

        public void OnNavigatingFrom(){}
    }
}
