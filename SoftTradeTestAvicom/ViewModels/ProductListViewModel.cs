using SoftTradeTestAvicom.Models;
using SoftTradeTestAvicom.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ProductListViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationManager _navigationManager;

        private readonly SoftTradeDbContext _db;

        private Product _selectedProduct;

        private string _oldView;

        private bool _showOkButton;

        public ProductListViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;

            Products = new ObservableCollection<Product>(_db.Products.ToList());
        }

        /// <summary>
        /// Флаг отображения кнопки для выбора записи
        /// </summary>
        public bool ShowOkButton
        {
            get => _showOkButton;
            set { _showOkButton = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Вернуться на предыдущий экран
        /// </summary>
        public Command GoBack =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = _oldView,
                    NavigationFrom = NavigationKeys.ProductListView
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            },
            obj => _oldView != null);

        /// <summary>
        /// Выбрать выделенную запись
        /// </summary>
        public Command Ok =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = _oldView,
                    NavigationFrom = NavigationKeys.ProductListView,
                    Arg = SelectedProduct
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            },
            obj => SelectedProduct != null);

        /// <summary>
        /// Список продуктов
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
        /// Вернуться на главное меню
        /// </summary>
        public Command GoMainMenu =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationFrom = NavigationKeys.ProductListView,
                    NavigationTo = NavigationKeys.MainMenuView
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            });

        /// <summary>
        /// Удалить выбранный продукт
        /// </summary>
        public Command Delete =>
            new(obj =>
            {
                if (obj is Product product)
                {
                    _ = Products.Remove(product);
                    _ = _db.Products.Remove(product);
                    _ = _db.SaveChanges();
                }
            },
            obj => Products.Count > 0 && SelectedProduct != null);

        /// <summary>
        /// Добавить новый продукт
        /// </summary>
        public Command Add =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ProductEditView,
                    NavigationFrom = NavigationKeys.ProductListView
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            });

        /// <summary>
        /// Редактирования выбранного продукта
        /// </summary>
        public Command Edit =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ProductEditView,
                    NavigationFrom = NavigationKeys.ProductListView,
                    Arg = SelectedProduct
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            },
            obj => SelectedProduct != null);

        /// <summary>
        /// Обновление списка
        /// </summary>
        public Command Refresh =>
            new(obj =>
            {
                Products = new ObservableCollection<Product>(_db.Products.ToList());
                OnPropertyChanged(nameof(Products));
            });

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            SelectedProduct = null;
        }

        /// <summary>
        /// Отрабатывает при входе во View
        /// </summary>
        /// <param name="arg"></param>
        public void OnNavigatingTo(NavigationInput arg)
        {
            _oldView = arg.NavigationFrom;

            if (_oldView == NavigationKeys.ProductEditView)
            {
                Products = new ObservableCollection<Product>(_db.Products.ToList());
                OnPropertyChanged(nameof(Products));
            }
            else if (_oldView == NavigationKeys.ClientEditView)
            {
                ShowOkButton = true;
            }
        }
    }
}
