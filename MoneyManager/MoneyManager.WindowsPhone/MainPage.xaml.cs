#region

using System.Linq;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Business.Logic;
using MoneyManager.Common;
using MoneyManager.Foundation;
using MoneyManager.Views;

#endregion

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        private readonly NavigationHelper navigationHelper;

        public MainPage()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private void AddAccountClick(object sender, RoutedEventArgs e)
        {
            AccountLogic.PrepareAddAccount();
            Frame.Navigate(typeof (AddAccount));
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (SettingsOverview));
        }

        private void GoToAbout(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (About));
        }

        private void AddIncomeClick(object sender, RoutedEventArgs e)
        {
            AddTransaction(TransactionType.Income);
        }

        private void AddSpendingClick(object sender, RoutedEventArgs e)
        {
            AddTransaction(TransactionType.Spending);
        }

        private void AddTransferClick(object sender, RoutedEventArgs e)
        {
            AddTransaction(TransactionType.Transfer);
        }

        private static void AddTransaction(TransactionType type)
        {
            TransactionLogic.GoToAddTransaction(type);
            ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
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