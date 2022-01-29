using System;

namespace MoneyFox.Win.Services
{
    public interface INavigationService
    {
        void Initialize(object frame);

        bool Navigate<TViewModel>(object parameter = null);

        bool Navigate(Type viewModelType, object parameter = null);

        bool CanGoBack { get; }

        bool GoBack();

        bool GoForward();
    }
}