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

        public ProductListViewModel(INavigationManager navigationManager,
            SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;

            Products = new ObservableCollection<Product>(_db.Products.ToList());
        }

        public ObservableCollection<Product> Products { get; set; }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        public Command GoMainMenu =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.MainMenuView);
            });

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

        public Command Add =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ProductEditView);
            });

        public Command Edit =>
            new(obj =>
            {
                _navigationManager.Navigate(NavigationKeys.ProductEditView, SelectedProduct);
            },
            obj => SelectedProduct != null);

        public Command Refresh =>
            new(obj =>
            {
                Products = new ObservableCollection<Product>(_db.Products.ToList());
                OnPropertyChanged(nameof(Products));
            });

        public void OnNavigatingFrom()
        {
            
        }

        public void OnNavigatingTo(object arg)
        {
            if (arg is Product)
            {
                Products = new ObservableCollection<Product>(_db.Products.ToList());
                OnPropertyChanged(nameof(Products));
            }
        }
    }
}
