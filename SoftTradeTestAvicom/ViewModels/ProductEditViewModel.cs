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

        public ProductEditViewModel(SoftTradeDbContext softTradeDbContext,
            INavigationManager navigationManager)
        {
            _db = softTradeDbContext;
            _navigationManager = navigationManager;
        }

        public static List<string> ProductType => new() { "Подписка", "Постоянная лицензия" };

        public static List<string> SubscriptionPeriod => new() { "Месяц", "Квартал", "Год" };

        public Command Cancel =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ProductListView);
            });

        public Command Ok =>
            new(obj =>
            {
                if (_addNew)
                {
                    _ = _db.Products.Add(Product);
                    _ = _db.SaveChanges();
                    _navigationManager.Navigate(NavigationKeys.ProductListView, Product);
                }
                else
                {
                    _ = _db.Products.Update(Product);
                    _ = _db.SaveChanges();
                    _navigationManager.Navigate(NavigationKeys.ProductListView, Product);
                }
            });

        public Product Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(); }
        }

        public void OnNavigatingFrom()
        {
            Product = null;
        }

        public void OnNavigatingTo(object arg)
        {
            if (arg is Product product)
            {
                Product = product;
                _addNew = false;
            }
            else
            {
                _addNew = true;
                Product = new Product
                {
                    SubscriptionPeriod = SubscriptionPeriod[0],
                    Type = ProductType[0]
                };
            }
        }
    }
}
