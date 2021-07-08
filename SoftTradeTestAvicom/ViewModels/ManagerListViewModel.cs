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

        private Manager _selectedManager;

        private bool _selectView;

        public ManagerListViewModel(SoftTradeDbContext dbContext, INavigationManager navigationManager)
        {
            _db = dbContext;
            _navigationManager = navigationManager;

            Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
        }

        public bool SelectView
        {
            get => _selectView;
            set { _selectView = value; OnPropertyChanged(); }
        }

        public Command Select =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ClientEditView);
            });

        public ObservableCollection<Manager> Managers { get; set; }

        public Manager SelectedManager
        {
            get => _selectedManager;
            set { _selectedManager = value; OnPropertyChanged(); }
        }

        public Command GoBack =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.MainMenuView);
            });

        public Command Delete =>
            new(obj =>
            {
                if (obj is Manager manager)
                {
                    _ = Managers.Remove(manager);
                    _ = _db.Managers.Remove(manager);
                    _ = _db.SaveChanges();
                }
            },
            obj => Managers.Count > 0 && SelectedManager != null);

        public Command Add =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ManagerEditView);
            });

        public Command Edit =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ManagerEditView, SelectedManager);
            },
            obj => SelectedManager != null);

        public Command Refresh =>
            new(obj =>
            {
                Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
                OnPropertyChanged(nameof(Managers));
            });

        public void OnNavigatingTo(object arg)
        {
            if (arg is Manager)
            {
                Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
                OnPropertyChanged(nameof(Managers));
            }
            else if(arg is Client)
            {
                SelectView = true;
            }
        }

        public void OnNavigatingFrom() { }
    }
}
