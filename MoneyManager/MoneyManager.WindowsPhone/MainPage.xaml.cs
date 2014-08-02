using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        private readonly Parameters parameters = new Parameters();

        public MainPage()
        {
            InitializeComponent();
        }

        private Account selectedAccount
        {
            get { return new ViewModelLocator().Main.SelectedAccount; }
            set { new ViewModelLocator().Main.SelectedAccount = value; }
        }

        private FinancialTransaction selectedTransaction
        {
            get { return new ViewModelLocator().Main.SelectedTransaction; }
            set { new ViewModelLocator().Main.SelectedTransaction = value; }
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            selectedAccount = new Account();
            Frame.Navigate(typeof(AddAccount));
        }

        private void AddTransaction_OnClick(object sender, RoutedEventArgs e)
        {
            selectedTransaction = new FinancialTransaction();

            switch ((e.OriginalSource as MenuFlyoutItem).Text)
            {
                case "spending":
                    parameters.TransactionType = TransactionType.Spending;
                    break;

                case "income":
                    parameters.TransactionType = TransactionType.Income;
                    break;

                case "transfer":
                    parameters.TransactionType = TransactionType.Transfer;
                    break;

                case "refund":
                    parameters.TransactionType = TransactionType.Refund;
                    break;

                default:
                    break;
            }

            Frame.Navigate(typeof(AddTransaction), parameters);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsOverview));
        }
    }
}