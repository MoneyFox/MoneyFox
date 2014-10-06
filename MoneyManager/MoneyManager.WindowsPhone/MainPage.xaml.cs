using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;
using Windows.UI.Xaml.Navigation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.Views;

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        private NavigationHelper navigationHelper;

        public MainPage()
        {
            this.InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
            set { ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount = value; }
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            SelectedAccount = new Account
            {
                Currency = "CHF"
            };
            ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit = false;
            Frame.Navigate(typeof(AddAccount));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsOverview));
        }

        private void RecurringTransactions_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecurringTransactionList));
        }

        private void AddSpendingClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Spending);
        }

        private void AddIncomeClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Income);
        }

        private void GoToAbout(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
