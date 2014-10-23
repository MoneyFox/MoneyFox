using Windows.UI.Xaml;
using MoneyManager.Business.Src;
using MoneyManager.Common;
using MoneyManager.Foundation;

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

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private void AddSpendingClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Spending);
        }

        private void AddIncomeClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Income);
        }

        private void AddTransferClick(object sender, RoutedEventArgs e)
        {
            TransactionHelper.GoToAddTransaction(TransactionType.Transfer);
        }
    }
}