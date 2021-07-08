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

        private string _oldView;

        public ClientEditViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;
        }

        /// <summary>
        /// Редактируемый клиент
        /// </summary>
        public Client Client
        {
            get => _client;
            set { _client = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Отменить изменения
        /// </summary>
        public Command Cancel =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationFrom = NavigationKeys.ClientEditView,
                    NavigationTo = _oldView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public Command Ok =>
            new(obj =>
            {
                if (_addNew)
                {
                    _ = _db.Clients.Add(Client);
                    _ = _db.SaveChanges();
                }
                else
                {
                    _ = _db.Clients.Update(Client);
                    _ = _db.SaveChanges();
                }
                var input = new NavigationInput
                {
                    Arg = Client,
                    NavigationFrom = NavigationKeys.ClientEditView,
                    NavigationTo = _oldView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            Client = null;
            _oldView = null;
        }

        /// <summary>
        /// Отрабатывает при входе во View
        /// </summary>
        /// <param name="arg"></param>
        public void OnNavigatingTo(NavigationInput arg)
        {
            _oldView = arg.NavigationFrom;

            if (arg.Arg is Client client)
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
