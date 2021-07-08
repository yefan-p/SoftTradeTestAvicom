using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using SoftTradeTestAvicom.Utils;
using SoftTradeTestAvicom.Models;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ClientListViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationManager _navigationManager;

        private readonly SoftTradeDbContext _db;

        private Client _selectedClient;

        public ClientListViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;

            Clients = new ObservableCollection<Client>(_db.Clients.ToList());
        }

        public ObservableCollection<Client> Clients { get; set; }

        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public Command GoMainMenu =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.MainMenuView);
            });

        public Command Delete =>
            new(obj =>
            {
                if (obj is Client client)
                {
                    _ = Clients.Remove(client);
                    _ = _db.Clients.Remove(client);
                    _ = _db.SaveChanges();
                }
            },
            obj => Clients.Count > 0 && SelectedClient != null);

        public Command Add =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ClientEditView);
            });

        public Command Edit =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ClientEditView, SelectedClient);
            },
            obj => SelectedClient != null);

        public Command Refresh =>
            new(obj =>
            {
                Clients = new ObservableCollection<Client>(_db.Clients.ToList());
                OnPropertyChanged(nameof(Clients));
            });

        public void OnNavigatingFrom()
        {

        }

        public void OnNavigatingTo(object arg)
        {
            if (arg is Client)
            {
                Clients = new ObservableCollection<Client>(_db.Clients.ToList());
                OnPropertyChanged(nameof(Clients));
            }
        }
    }
}
