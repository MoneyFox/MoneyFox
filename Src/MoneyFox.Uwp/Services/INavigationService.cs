using System;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.Services
{
    public interface INavigationService
    {
        public interface INavigationService
        {
            bool IsMainView { get; }

            bool CanGoBack { get; }

            void Initialize(object frame);

            bool Navigate<TViewModel>(object parameter = null);
            bool Navigate(Type viewModelType, object parameter = null);

            Task<int> CreateNewViewAsync<TViewModel>(object parameter = null);
            Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null);

            bool GoBack();
            bool GoForward();

            Task CloseViewAsync();
        }
    }
}
