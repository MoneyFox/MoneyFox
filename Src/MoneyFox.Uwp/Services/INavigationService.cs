using System;

namespace MoneyFox.Uwp.Services
{
    public interface INavigationService
    {
        bool IsMainView { get; }

        void Initialize(object frame);

        bool Navigate<TViewModel>(object parameter = null);

        bool Navigate(Type viewModelType, object parameter = null);

        bool CanGoBack { get; }

        bool GoBack();

        bool GoForward();
    }
}
