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
    public class ClientsTests
    {
        /// <summary>
        /// todo: сделать фейковый контекст для тестов
        /// </summary>
        private SoftTradeDbContext _db;

        private ClientEditViewModel _viewModel;

        private NavigationMoq _navigation;

        [Fact]
        public void ClientInit()
        {
            _navigation = new NavigationMoq();
            _db = new SoftTradeDbContext();
            _viewModel = new ClientEditViewModel(_navigation, _db);
            var input = new NavigationInput();

            Manager manager = _db.Managers.First();

            _viewModel.OnNavigatingTo(input);
            _viewModel.Client.Name = "TestClient";
            _viewModel.Client.Manager = manager;

            Assert.Equal("TestClient", _viewModel.Client.Name);
        }

        [Theory]
        [InlineData(true)]
        public void ClientAdd(bool deleteAfter)
        {
            ClientInit();
            _viewModel.Ok.Execute(new object());

            var query =
                from el in _db.Clients
                where el.Name == "TestClient"
                select el;

            Client fromDb = query.SingleOrDefault();
            Assert.NotNull(fromDb);

            if (deleteAfter)
            {
                _db.Clients.Remove(fromDb);
                _db.SaveChanges();
            }
        }

        [Fact]
        public void ClientEdit()
        {
            ClientAdd(false);

            var query =
                from el in _db.Clients
                where el.Name == "TestClient"
                select el;

            Client fromDb = query.Single();
            var input = new NavigationInput
            {
                Arg = fromDb
            };
            fromDb.Name = "TestClientEdit";

            _viewModel.OnNavigatingTo(input);
            _viewModel.Ok.Execute(new object());

            query =
                from el in _db.Clients
                where el.Name == "TestClientEdit"
                select el;

            fromDb = query.SingleOrDefault();
            Assert.NotNull(fromDb);

            _db.Clients.Remove(fromDb);
            _db.SaveChanges();
        }

        [Fact]
        public void ClientDelete()
        {
            ClientAdd(false);
            var query =
                from el in _db.Clients
                where el.Name == "TestClient"
                select el;
            Client fromDb = query.Single();

            var listViewModel = new ClientListViewModel(_navigation, _db);

            Assert.Contains(fromDb, listViewModel.Clients);

            listViewModel.Delete.Execute(fromDb);

            Assert.DoesNotContain(fromDb, listViewModel.Clients);
        }
    }
}
