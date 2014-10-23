using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Src;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.ViewModels;

namespace MoneyManager.UserControls
{
    public sealed partial class TransactionListUserControl
    {
        public TransactionListUserControl()
        {
            InitializeComponent();
        }

        public TransactionDataAccess TransactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public AddTransactionViewModel AddTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        private void EditTransaction(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null) return;

            TransactionHelper.GoToEdit(transaction);
        }

        private void DeleteTransaction(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null) return;

            TransactionData.Delete(transaction);
        }

        private void OpenContextMenu(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void EditTransactionOnSelection(object sender, SelectionChangedEventArgs e)
        {
            //todo: fix multiple navigation. Selection Changed gets triggered from jumplist
            //if (TransactionListView.SelectedItem != null)
            //{
            //    TransactionData.SelectedTransaction = TransactionListView.SelectedItem as FinancialTransaction;
            //    AddTransactionView.IsEdit = true;

            //    ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction));
            //    TransactionListView.SelectedItem = null;
            //}
        }
    }
}