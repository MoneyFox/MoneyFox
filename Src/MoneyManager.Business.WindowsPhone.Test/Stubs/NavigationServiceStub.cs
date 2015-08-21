using GalaSoft.MvvmLight.Views;

namespace MoneyManager.Business.WindowsPhone.Test.Stubs
{
    //TODO: Use Moq
    public class NavigationServiceStub : INavigationService
    {
        public void GoBack()
        {
        }

        public void NavigateTo(string pageKey)
        {
        }

        public void NavigateTo(string pageKey, object parameter)
        {
        }

        public string CurrentPageKey { get; }
    }
}