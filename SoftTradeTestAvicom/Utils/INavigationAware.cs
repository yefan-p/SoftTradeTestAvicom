namespace SoftTradeTestAvicom.Utils
{
    public interface INavigationAware
    {
        void OnNavigatingTo(NavigationInput arg);
        void OnNavigatingFrom();
    }
}
