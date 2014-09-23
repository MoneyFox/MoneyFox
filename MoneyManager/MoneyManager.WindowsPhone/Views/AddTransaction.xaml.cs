using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;
using MoneyManager.ViewModels;

namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        private readonly NavigationHelper navigationHelper;

        public AddTransaction()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<AddTransactionViewModel>().Save();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<AddTransactionViewModel>().Cancel();
        }
    }
}