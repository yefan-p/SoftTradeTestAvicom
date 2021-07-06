﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

using SoftTradeTestAvicom.Utils;

namespace SoftTradeTestAvicom.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Переключение между различными представлениями
        /// </summary>
        public INavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Свойство обновлено
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Функция вызова события "Свойство обновленно"
        /// </summary>
        /// <param name="propertyName">Имя вызывающего метода или свойства. Заполняется автоматически</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
