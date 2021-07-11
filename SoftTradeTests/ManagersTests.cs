using System;
using Xunit;

using System.Windows.Threading;
using System.Windows.Controls;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

using SoftTradeTests.Moq;
using SoftTradeTestAvicom.Models;
using SoftTradeTestAvicom.ViewModels;
using SoftTradeTestAvicom.Utils;

namespace SoftTradeTests
{
    public class ManagersTests
    {
        /// <summary>
        /// todo: сделать фейковый контекст для тестов
        /// </summary>
        private SoftTradeDbContext _db;

        private ManagerEditViewModel _viewModel;

        private NavigationMoq _navigation;

        [Fact]
        public void ManagerInit()
        {
            _navigation = new NavigationMoq();
            _db = new SoftTradeDbContext();
            _viewModel = new ManagerEditViewModel(_navigation, _db);
            var input = new NavigationInput();

            _viewModel.OnNavigatingTo(input);
            _viewModel.Manager.Name = "TestManager";

            Assert.Equal("TestManager", _viewModel.Manager.Name);
        }

        [Theory]
        [InlineData(true)]
        public Manager ManagerAdd(bool deleteAfter)
        {
            ManagerInit();
            _viewModel.Ok.Execute(new object());

            var query =
                from el in _db.Managers
                where el.Name == "TestManager"
                select el;

            Manager managerFromDb = query.SingleOrDefault();
            Assert.NotNull(managerFromDb);

            if (deleteAfter)
            {
                _db.Managers.Remove(managerFromDb);
                _db.SaveChanges();
                return null;
            }
            return managerFromDb;
        }

        [Fact]
        public void ManagerEdit()
        {
            ManagerAdd(false);

            var query =
                from el in _db.Managers
                where el.Name == "TestManager"
                select el;

            Manager managerFromDb = query.Single();
            var input = new NavigationInput
            {
                Arg = managerFromDb
            };
            managerFromDb.Name = "TestManagerEdit";

            _viewModel.OnNavigatingTo(input);
            _viewModel.Ok.Execute(new object());

            query =
                from el in _db.Managers
                where el.Name == "TestManagerEdit"
                select el;

            managerFromDb = query.SingleOrDefault();
            Assert.NotNull(managerFromDb);

            _db.Managers.Remove(managerFromDb);
            _db.SaveChanges();
        }

        [Fact]
        public void ManagerDelete()
        {
            ManagerAdd(false);
            var query =
                from el in _db.Managers
                where el.Name == "TestManager"
                select el;
            Manager managerFromDb = query.Single();

            var listViewModel = new ManagerListViewModel(_db, _navigation);

            Assert.Contains(managerFromDb, listViewModel.Managers);

            listViewModel.Delete.Execute(managerFromDb);

            Assert.DoesNotContain(managerFromDb, listViewModel.Managers);
        }
    }
}
