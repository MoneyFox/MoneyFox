namespace MoneyFox.Application.Common.Interfaces
{
    public interface INavigationService
    {
        string CurrentPageKey { get; }

        void GoBack();

        void NavigateTo(string pageKey);

        void NavigateTo(string pageKey, object parameter);

        void NavigateToModal(string pageKey);

        void NavigateToModal(string pageKey, object parameter);
    }
}
