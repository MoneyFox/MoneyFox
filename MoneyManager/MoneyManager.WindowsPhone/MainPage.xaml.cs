using System.Globalization;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        private NavigationHelper navigationHelper;

        public MainPage()
        {
            InitializeComponent();

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

        private void AddAccountClick(object sender, RoutedEventArgs e)
        {
            SelectedAccount = new Account();
            ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit = false;
            Frame.Navigate(typeof(AddAccount));
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsOverview));
        }

        private void RecurringTransactionsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecurringTransactionList));
        }

        private void GoToAbout(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void AddIncomeClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Income);
        }

        private void AddSpendingClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Spending);
        }

        private void AddTransferClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Transfer);
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

        #endregion NavigationHelper registration
    }
}