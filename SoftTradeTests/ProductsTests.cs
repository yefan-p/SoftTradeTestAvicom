using System;
using Xunit;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTests.Moq;
using SoftTradeTestAvicom.Models;
using SoftTradeTestAvicom.ViewModels;
using SoftTradeTestAvicom.Utils;

namespace SoftTradeTests
{
    public class ProductsTests
    {        
        /// <summary>
        /// todo: сделать фейковый контекст для тестов
        /// </summary>
        private SoftTradeDbContext _db;

        private ProductEditViewModel _viewModel;

        private NavigationMoq _navigation;

        [Fact]
        public void ProductInit()
        {
            _navigation = new NavigationMoq();
            _db = new SoftTradeDbContext();
            _viewModel = new ProductEditViewModel(_db, _navigation);
            var input = new NavigationInput();

            _viewModel.OnNavigatingTo(input);
            _viewModel.Product.Name = "TestProduct";

            Assert.Equal("TestProduct", _viewModel.Product.Name);
        }

        [Theory]
        [InlineData(true)]
        public void ProductAdd(bool deleteAfter)
        {
            ProductInit();
            _viewModel.Ok.Execute(new object());

            var query =
                from el in _db.Products
                where el.Name == "TestProduct"
                select el;

            Product productFromDb = query.SingleOrDefault();
            Assert.NotNull(productFromDb);

            if (deleteAfter)
            {
                _db.Products.Remove(productFromDb);
                _db.SaveChanges();
            }
        }

        [Fact]
        public void ProductEdit()
        {
            ProductAdd(false);

            var query =
                from el in _db.Products
                where el.Name == "TestProduct"
                select el;

            Product productFromDb = query.Single();
            var input = new NavigationInput
            {
                Arg = productFromDb
            };
            productFromDb.Name = "TestProductEdit";

            _viewModel.OnNavigatingTo(input);
            _viewModel.Ok.Execute(new object());

            query =
                from el in _db.Products
                where el.Name == "TestProductEdit"
                select el;

            productFromDb = query.SingleOrDefault();
            Assert.NotNull(productFromDb);

            _db.Products.Remove(productFromDb);
            _db.SaveChanges();
        }

        [Fact]
        public void ProductDelete()
        {
            ProductAdd(false);
            var query =
                from el in _db.Products
                where el.Name == "TestProduct"
                select el;
            Product productFromDb = query.Single();

            var listViewModel = new ProductListViewModel(_navigation, _db);

            Assert.Contains(productFromDb, listViewModel.Products);

            listViewModel.Delete.Execute(productFromDb);

            Assert.DoesNotContain(productFromDb, listViewModel.Products);
        }
    }
}
