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

        private string _oldView;

        public ClientListViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;

            Clients = new ObservableCollection<Client>(_db.Clients.ToList());
        }

        /// <summary>
        /// Список клиентов
        /// </summary>
        public ObservableCollection<Client> Clients { get; set; }

        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Вернуться на главное меню
        /// </summary>
        public Command GoMainMenu =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationFrom = NavigationKeys.ClientListView,
                    NavigationTo = NavigationKeys.MainMenuView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Удалить выбранного клиента
        /// </summary>
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

        /// <summary>
        /// Добавить нового клиента
        /// </summary>
        public Command Add =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationFrom = NavigationKeys.ClientListView,
                    NavigationTo = NavigationKeys.ClientEditView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Редактирования выбранного клиента
        /// </summary>
        public Command Edit =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    Arg = SelectedClient,
                    NavigationFrom = NavigationKeys.ClientListView,
                    NavigationTo = NavigationKeys.ClientEditView
                };
                _navigationManager.Navigate(input);
            },
            obj => SelectedClient != null);

        /// <summary>
        /// Обновление списка
        /// </summary>
        public Command Refresh =>
            new(obj =>
            {
                Clients = new ObservableCollection<Client>(_db.Clients.ToList());
                OnPropertyChanged(nameof(Clients));
            });

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            _oldView = null;
        }

        /// <summary>
        /// Отрабатывает при входе во View
        /// </summary>
        /// <param name="arg"></param>
        public void OnNavigatingTo(NavigationInput arg)
        {
            _oldView = arg.NavigationFrom;

            if (_oldView == NavigationKeys.ClientEditView)
            {
                Clients = new ObservableCollection<Client>(_db.Clients.ToList());
                OnPropertyChanged(nameof(Clients));
            }
        }
    }
}
