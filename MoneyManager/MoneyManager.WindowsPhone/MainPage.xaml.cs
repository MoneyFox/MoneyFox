using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.Views;
using Windows.UI.Xaml;

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
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
            Frame.Navigate(typeof(AddAccount));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsOverview));
        }

        private void AddSpendingClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Spending);
        }

        private void AddIncomeClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Income);
        }
    }
}