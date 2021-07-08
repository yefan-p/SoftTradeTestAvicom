using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftTradeTestAvicom.Models;
using SoftTradeTestAvicom.Utils;

namespace SoftTradeTestAvicom.ViewModels
{
    public class ManagerListViewModel : BaseViewModel, INavigationAware
    {
        private readonly SoftTradeDbContext _db;

        private readonly INavigationManager _navigationManager;

        private Manager _selectedManager;

        private bool _showOkButton;

        private string _oldView;

        public ManagerListViewModel(SoftTradeDbContext dbContext, INavigationManager navigationManager)
        {
            _db = dbContext;
            _navigationManager = navigationManager;

            Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
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
        /// Выбрать выделенную запись
        /// </summary>
        public Command Ok =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = _oldView,
                    NavigationFrom = NavigationKeys.ManagerListView,
                    Arg = SelectedManager
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            },
            obj => SelectedManager != null);

        /// <summary>
        /// Список менеджеров
        /// </summary>
        public ObservableCollection<Manager> Managers { get; set; }

        /// <summary>
        /// Выбранный менеджер
        /// </summary>
        public Manager SelectedManager
        {
            get => _selectedManager;
            set { _selectedManager = value; OnPropertyChanged(); }
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
                    NavigationFrom = NavigationKeys.ManagerListView
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            },
            obj => _oldView != null);

        /// <summary>
        /// Вернуться на главное меню
        /// </summary>
        public Command GoMain =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.MainMenuView,
                    NavigationFrom = NavigationKeys.ManagerListView
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            });

        /// <summary>
        /// Удалить выбранного менеджера
        /// </summary>
        public Command Delete =>
            new(obj =>
            {
                if (obj is Manager manager)
                {
                    _ = Managers.Remove(manager);
                    _ = _db.Managers.Remove(manager);
                    _ = _db.SaveChanges();
                }
            },
            obj => Managers.Count > 0 && SelectedManager != null);

        /// <summary>
        /// Добавить нового менеджера
        /// </summary>
        public Command Add =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ManagerEditView,
                    NavigationFrom = NavigationKeys.ManagerListView
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            });

        /// <summary>
        /// Редактирование выбранного менеджера
        /// </summary>
        public Command Edit =>
            new(obj =>
            {
                var input = new NavigationInput
                {
                    NavigationTo = NavigationKeys.ManagerEditView,
                    NavigationFrom = NavigationKeys.ManagerListView,
                    Arg = SelectedManager
                };
                _navigationManager.Navigate(input);
                ShowOkButton = false;
            },
            obj => SelectedManager != null);

        /// <summary>
        /// Обновление списка
        /// </summary>
        public Command Refresh =>
            new(obj =>
            {
                Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
                OnPropertyChanged(nameof(Managers));
            });

        /// <summary>
        /// Отрабатывает при входе во View
        /// </summary>
        /// <param name="arg"></param>
        public void OnNavigatingTo(NavigationInput arg)
        {
            _oldView = arg.NavigationFrom;

            if (_oldView == NavigationKeys.ManagerEditView)
            {
                Managers = new ObservableCollection<Manager>(_db.Managers.ToList());
                OnPropertyChanged(nameof(Managers));
            }
            else if (_oldView == NavigationKeys.ClientEditView)
            {
                ShowOkButton = true;
            }
        }

        /// <summary>
        /// Отрабатывает при переходе со View
        /// </summary>
        public void OnNavigatingFrom()
        {
            SelectedManager = null;
        }
    }
}
