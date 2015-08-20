using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Windows.Views;

namespace MoneyManager.Windows.Controls
{
    public partial class TransactionListUserControl
    {
        public TransactionListUserControl()
        {
            InitializeComponent();

            //TODO: Handle in View Model
            ServiceLocator.Current.GetInstance<BalanceViewModel>().IsTransactionView = true;
        }

        private void EditTransaction(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null)
            {
                return;
            }

            TransactionLogic.PrepareEdit(transaction);
            ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
        }

        private async void DeleteTransaction(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null)
            {
                return;
            }

            await TransactionLogic.DeleteTransaction(transaction);
            AddTransactionView.IsNavigationBlocked = false;
        }

        private void OpenContextMenu(object sender, HoldingRoutedEventArgs e)
        {
            AddTransactionView.IsNavigationBlocked = true;
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        //TODO: Handle in View Model
        private void UnloadPage(object sender, RoutedEventArgs e)
        {
            BalanceView.IsTransactionView = false;
            AddTransactionView.IsNavigationBlocked = true;
            BalanceView.UpdateBalance();
        }

        //TODO: Handle in View Model
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            AddTransactionView.IsNavigationBlocked = false;
            ListViewTransactions.SelectedItem = null;
        }

        //TODO: Handle in View Model
        private void LoadDetails(object sender, SelectionChangedEventArgs e)
        {
            if (!AddTransactionView.IsNavigationBlocked && ListViewTransactions.SelectedItem != null)
            {
                TransactionRepository.Selected = ListViewTransactions.SelectedItem as FinancialTransaction;

                TransactionLogic.PrepareEdit(TransactionRepository.Selected);

                ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
                ListViewTransactions.SelectedItem = null;
            }
        }

        #region Properties

        public ITransactionRepository TransactionRepository => ServiceLocator.Current.GetInstance<ITransactionRepository>();

        public AddTransactionViewModel AddTransactionView => ServiceLocator.Current.GetInstance<AddTransactionViewModel>();

        public BalanceViewModel BalanceView => ServiceLocator.Current.GetInstance<BalanceViewModel>();

        #endregion
    }
}