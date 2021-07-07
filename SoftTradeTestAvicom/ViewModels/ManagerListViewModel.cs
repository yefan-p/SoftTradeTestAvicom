using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Models;
using SoftTradeTestAvicom.Utils;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ManagerListViewModel : BaseViewModel, INavigationAware
    {
        private readonly SoftTradeDbContext _db;

        private readonly INavigationManager _navigationManager;
  
        public ManagerListViewModel(SoftTradeDbContext dbContext, INavigationManager navigationManager)
        {
            _db = dbContext;
            Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
            _navigationManager = navigationManager;
        }

        public ObservableCollection<Manager> Managers { get; set; }

        private Manager _selectedManager;

        public Manager SelectedManager 
        {
            get { return _selectedManager; } 
            set { _selectedManager = value; OnPropertyChanged(); }
        }

        private Command _goMainMenu;

        public Command GoMainMenu
        {
            get
            {
                return _goMainMenu ??
                    (_goMainMenu = new Command(obj =>
                    {
                        _navigationManager.Navigate(NavigationKeys.MainMenuView);
                    }));
            }
        }

        private Command _delete;

        public Command Delete
        {
            get
            {
                return _delete ??
                  (_delete = new Command(obj =>
                  {
                      Manager manager = obj as Manager;
                      if (manager != null)
                      {
                          Managers.Remove(manager);
                          _db.Managers.Remove(manager);
                          _db.SaveChanges();
                      }
                  },
                 (obj) => Managers.Count > 0));
            }
        }

        private Command _add;

        public Command Add
        {
            get
            {
                return _add ??
                    (_add = new Command(obj =>
                    {
                        _navigationManager.Navigate(NavigationKeys.ManagerEditView);
                    }));
            }
        }

        private Command _edit;

        public Command Edit
        {
            get
            {
                return _edit ??
                    (_edit = new Command(obj =>
                    {
                        _navigationManager.Navigate(NavigationKeys.ManagerEditView, SelectedManager);
                        Managers.Remove(SelectedManager);
                    }));
            }
        }

        private Command _refresh;

        public Command Refresh
        {
            get
            {
                return _refresh ??
                    (_refresh = new Command(obj =>
                    {
                        Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
                        OnPropertyChanged("Managers");
                    }));
            }
        }

        public void OnNavigatingTo(object arg)
        {
            if (arg != null)
            {
                Manager manager = (Manager)arg;
                Managers.Add(manager);
            }
        }

        public void OnNavigatingFrom(){}
    }
}
