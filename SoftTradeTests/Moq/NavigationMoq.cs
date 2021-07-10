using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SoftTradeTestAvicom.Utils;

namespace SoftTradeTests.Moq
{
    public class NavigationMoq : INavigationManager
    {
        public void Navigate(NavigationInput arg)
        {
            
        }

        public void Register<TViewModel, TView>(TViewModel viewModel, string navigationKey)
            where TViewModel : class
            where TView : FrameworkElement
        {
            throw new NotImplementedException();
        }
    }
}
