﻿namespace SoftTradeTestAvicom.Utils
{
    public interface INavigationAware
    {
        void OnNavigatingTo(object arg);
        void OnNavigatingFrom();
    }
}
