using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Utils;
using SoftTradeTestAvicom.Models;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ProductEditViewModel : BaseViewModel, INavigationAware
    {
        private readonly SoftTradeDbContext _db;

        private readonly INavigationManager _navigationManager;

        private Product _product;

        private bool _addNew;

        private string _oldView;

        private bool _showSubscripPeriod;

        public ProductEditViewModel(SoftTradeDbContext softTradeDbContext,
            INavigationManager navigationManager)
        {
            _db = softTradeDbContext;
            _navigationManager = navigationManager;
        }

        public static List<string> ProductType => new() { "Подписка", "Постоянная лицензия" };

        public static List<string> SubscriptionPeriod => new() { "Месяц", "Квартал", "Год" };

        /// <summary>
        /// Флаг отображения выбора периода подписки
        /// </summary>
        public bool ShowSubscripPeriod
        {
            get => _showSubscripPeriod;
            set { _showSubscripPeriod = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Выбранный продукт
        /// </summary>
        public string SelectedProductType
        {
            get => Product.Type;
            set 
            { 
                Product.Type = value;
                if (value == ProductType[1])
                {
                    ShowSubscripPeriod = false;
                    Product.SubscriptionPeriod = null;
                }
                else 
                {
                    ShowSubscripPeriod = true;
                    Product.SubscriptionPeriod = SubscriptionPeriod[0];
                }
                OnPropertyChanged(nameof(Product));
            }
        }

        /// <summary>
        /// Отменить изменения
        /// </summary>
        public Command Cancel =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = _oldView,
                    NavigationFrom = NavigationKeys.ProductEditView
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
                    _ = _db.Products.Add(Product);
                    _ = _db.SaveChanges();
                }
                else
                {
                    var query =
                        from el in _db.Products
                        where el.Id == Product.Id
                        select el;

                    Product productDb = query.Single();
                    productDb.Name = Product.Name;
                    productDb.Price = Product.Price;
                    productDb.SubscriptionPeriod = Product.SubscriptionPeriod;
                    productDb.Type = Product.Type;

                    _ = _db.Products.Update(productDb);
                    _ = _db.SaveChanges();
                }
                var input = new NavigationInput
                {
                    Arg = Product,
                    NavigationFrom = NavigationKeys.ProductEditView,
                    NavigationTo = _oldView
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Редактируемый продукт
        /// </summary>
        public Product Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            _oldView = null;
            Product = null;
        }

        /// <summary>
        /// Отрабатывает при входе во View
        /// </summary>
        /// <param name="arg"></param>
        public void OnNavigatingTo(NavigationInput arg)
        {
            _oldView = arg.NavigationFrom;

            if (arg.Arg is Product product)
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Clients = product.Clients,
                    Price = product.Price,
                    SubscriptionPeriod = product.SubscriptionPeriod,
                    Type = product.Type
                };
                _addNew = false;
                SelectedProductType = product.Type;
            }
            else
            {
                _addNew = true;
                Product = new Product
                {
                    SubscriptionPeriod = SubscriptionPeriod[0],
                };
                SelectedProductType = ProductType[0];
            }
        }
    }
}
