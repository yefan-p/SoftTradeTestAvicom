using System.Windows;

namespace SoftTradeTestAvicom.Utils
{
    public interface INavigationManager
    {
        void Navigate(NavigationInput arg);

        void Register<TViewModel, TView>(TViewModel viewModel, string navigationKey)
            where TViewModel : class
            where TView : FrameworkElement;
    }
}
