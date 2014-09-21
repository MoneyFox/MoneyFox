using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;
using Windows.UI.Xaml.Navigation;
using MoneyManager.ViewModels;

namespace MoneyManager.Views
{
    public sealed partial class AddAccount
    {
        private readonly NavigationHelper navigationHelper;

        public AddAccount()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<AddAccountViewModel>().AddAccount();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<AddAccountViewModel>().Cancel();
        }

        #region NavigationHelper registration

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}