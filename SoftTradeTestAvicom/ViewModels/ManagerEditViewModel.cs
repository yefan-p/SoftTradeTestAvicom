using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Utils;
using SoftTradeTestAvicom.Models;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ManagerEditViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationManager _navigationManager;

        private readonly SoftTradeDbContext _db;

        private string _oldView;

        private Manager _manager;

        private bool _addNew;

        public ManagerEditViewModel(INavigationManager navigationManager, SoftTradeDbContext softTradeDbContext)
        {
            _navigationManager = navigationManager;
            _db = softTradeDbContext;
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
                    NavigationFrom = NavigationKeys.ManagerEditView
                };
                _navigationManager.Navigate(input);
            },
            obj => _oldView != null);

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public Command Ok =>
            new(obj =>
            {
                if (_addNew)
                {
                    _ = _db.Managers.Add(Manager);
                    _ = _db.SaveChanges();
                }
                else
                {
                    var query =
                        from el in _db.Managers
                        where el.Id == Manager.Id
                        select el;

                    Manager resultQuery = query.Single();
                    resultQuery.Name = Manager.Name;

                    _ = _db.Managers.Update(resultQuery);
                    _ = _db.SaveChanges();
                }
                var input = new NavigationInput
                {
                    NavigationFrom = NavigationKeys.ManagerEditView,
                    NavigationTo = _oldView,
                    Arg = Manager
                };
                _navigationManager.Navigate(input);
            });

        /// <summary>
        /// Редактируемый менеджер
        /// </summary>
        public Manager Manager
        {
            get => _manager;
            set { _manager = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Отрабатывает при входе во View
        /// </summary>
        /// <param name="arg"></param>
        public void OnNavigatingTo(NavigationInput arg)
        {
            _oldView = arg.NavigationFrom;

            if (arg.Arg is Manager manager && Manager != null)
            {
                Manager = new Manager()
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    Clients = manager.Clients
                };
                _addNew = false;
            }
            else
            {
                _addNew = true;
                Manager = new Manager();
            }
        }

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            _oldView = null;
            Manager = null;
        }
    }
}
