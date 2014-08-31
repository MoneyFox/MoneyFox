using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;
using MoneyManager.UserControls;
using MoneyManager.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class TransactionList
    {
        private readonly NavigationHelper navigationHelper;

        public TransactionList()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        #region NavigationHelper registration

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
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