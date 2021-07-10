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
    public class ManagerEditTests
    {
        [Fact]
        public void ManagerInit()
        {
            var navigator = new NavigationMoq();
            var context = new SoftTradeDbContext();

            var managerEdit = new ManagerEditViewModel(navigator, context);
            var input = new NavigationInput
            {
                Arg = new Manager
                {
                    Name = "TestManager"
                }
            };
            managerEdit.OnNavigatingTo(input);

            Assert.Equal("TestManager", managerEdit.Manager.Name);
        }

        [Fact]
        public void ManagerAdd()
        {
            var navigator = new NavigationMoq();
            var context = new SoftTradeDbContext();

            var managerEdit = new ManagerEditViewModel(navigator, context);
            var input = new NavigationInput
            {
                Arg = new Manager
                {
                    Name = "TestManager"
                }
            };
            managerEdit.OnNavigatingTo(input);

            Assert.Equal("TestManager", managerEdit.Manager.Name);

            managerEdit.Ok.Execute(new object());

            var query =
                from el in context.Managers
                where el.Name == "TestManager"
                select el;

            Manager managerFromDb = query.SingleOrDefault();
            Assert.NotNull(managerFromDb);

            context.Managers.Remove(managerFromDb);
            context.SaveChanges();
        }

        [Fact]
        public void ManagerEdit()
        {
            var navigator = new NavigationMoq();
            var context = new SoftTradeDbContext();

            var managerEdit = new ManagerEditViewModel(navigator, context);
            var input = new NavigationInput
            {
                Arg = new Manager
                {
                    Name = "TestManager"
                }
            };
            managerEdit.OnNavigatingTo(input);

            Assert.Equal("TestManager", managerEdit.Manager.Name);

            managerEdit.Ok.Execute(new object());
            managerEdit.Manager.Name = "TestManagerEdit";
            managerEdit.Ok.Execute(new object());

            var query =
                from el in context.Managers
                where el.Name == "TestManagerEdit"
                select el;

            Manager managerFromDb = query.SingleOrDefault();
            Assert.NotNull(managerFromDb);

            context.Managers.Remove(managerFromDb);
            context.SaveChanges();
        }
    }
}
