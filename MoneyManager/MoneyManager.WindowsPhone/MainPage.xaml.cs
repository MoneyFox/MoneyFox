using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.Views;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private Account selectedAccount
        {
            set { new ViewModelLocator().Main.SelectedAccount = value; }
        }

        private FinancialTransaction selectedTransaction
        {
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
            var viewModel = new ViewModelLocator().AddTransaction;

            switch ((e.OriginalSource as MenuFlyoutItem).Text)
            {
                case "spending":
                    viewModel.TransactionType = TransactionType.Spending;
                    viewModel.TransactionTitle = Utilities.GetTranslation("SpendingTitle");
                    break;

                case "income":
                    viewModel.TransactionType = TransactionType.Income;
                    viewModel.TransactionTitle = Utilities.GetTranslation("IncomeTitle");
                    break;

                case "transfer":
                    viewModel.TransactionType = TransactionType.Transfer;
                    viewModel.TransactionTitle = Utilities.GetTranslation("Transfer.Title");
                    break;

                case "refund":
                    viewModel.TransactionType = TransactionType.Refund;
                    viewModel.TransactionTitle = Utilities.GetTranslation("RefundTitle");
                    break;

                default:
                    break;
            }
            Frame.Navigate(typeof(AddTransaction));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsOverview));
        }
    }
}