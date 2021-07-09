using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Utils;
using SoftTradeTestAvicom.Models;
using System.Collections.ObjectModel;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ClientEditViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationManager _navigationManager;

        private readonly SoftTradeDbContext _db;

        private Client _client;

        private bool _addNew;

        private string _oldView;

        private Product _selectedProduct;

        public ClientEditViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;
            Products = new ObservableCollection<Product>();
        }

        /// <summary>
        /// Список продуктов клиента
        /// </summary>
        public ObservableCollection<Product> Products { get; set; }

        /// <summary>
        /// Выбранный продукт
        /// </summary>
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
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
        /// Добавить продукт клиенту
        /// </summary>
        public Command AddProduct =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ProductListView,
                    NavigationFrom = NavigationKeys.ClientEditView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Удалить продукт у клиента
        /// </summary>
        public Command DeleteProduct =>
            new(obj =>
            {
                foreach (Product item in Products)
                {
                    if (item.Id == SelectedProduct.Id)
                    {
                        Products.Remove(item);
                        OnPropertyChanged(nameof(Products));
                        break;
                    }                 
                }
            });

        /// <summary>
        /// Выбрать менеджера
        /// </summary>
        public Command SelectManager =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ManagerListView,
                    NavigationFrom = NavigationKeys.ClientEditView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Отменить изменения
        /// </summary>
        public Command Cancel =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationFrom = NavigationKeys.ClientEditView,
                    NavigationTo = NavigationKeys.ClientListView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public Command Ok =>
            new(obj =>
            {
                Client.Products = Products;
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
                    NavigationTo = NavigationKeys.ClientListView
                };
                _navigationManager.Navigate(input);
            },
            obj => Client.ManagerId != 0);

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            
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
                Products = new ObservableCollection<Product>(Client.Products);
            }
            else if (arg.Arg is Manager manager)
            {
                Client.ManagerId = manager.Id;
                Client.Manager = manager;
            }
            else if (arg.Arg is Product product && !Products.Contains(product))
            {
                Products.Add(product);
            }
            else if (Client == null || _oldView == NavigationKeys.ClientListView)
            {
                _addNew = true;
                Client = new Client();
                Products = new ObservableCollection<Product>();
            }
        }
    }
}
