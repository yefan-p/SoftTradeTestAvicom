using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Utils;
using SoftTradeTestAvicom.Models;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ClientEditViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationManager _navigationManager;

        private readonly SoftTradeDbContext _db;

        private Client _client;

        private bool _addNew;

        public ClientEditViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;
        }

        public Client Client
        {
            get => _client;
            set { _client = value; OnPropertyChanged(); }
        }

        public Command Cancel =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ClientListView);
            });

        public Command Ok =>
            new(obj =>
            {
                if (_addNew)
                {
                    _ = _db.Clients.Add(Client);
                    _ = _db.SaveChanges();
                    _navigationManager.Navigate(NavigationKeys.ClientListView, Client);
                }
                else
                {
                    _ = _db.Clients.Update(Client);
                    _ = _db.SaveChanges();
                    _navigationManager.Navigate(NavigationKeys.ClientListView, Client);
                }
            });

        public void OnNavigatingFrom()
        {
            Client = null;
        }

        public void OnNavigatingTo(object arg)
        {
            if (arg is Client client)
            {
                Client = client;
                _addNew = false;
            }
            else
            {
                _addNew = true;
                Client = new Client();
            }
        }
    }
}
