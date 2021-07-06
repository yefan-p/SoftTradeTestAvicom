using System.Windows;

namespace SoftTradeTestAvicom.Utils
{
    public interface INavigationManager
    {
        void Navigate(string navigationKey, object arg = null);

        void Register<TViewModel, TView>(TViewModel viewModel, string navigationKey)
            where TViewModel : class
            where TView : FrameworkElement;
    }
}
