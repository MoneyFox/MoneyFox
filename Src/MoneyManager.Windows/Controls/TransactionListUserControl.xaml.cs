using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Windows.Controls
{
    public partial class TransactionListUserControl
    {
        public TransactionListUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<TransactionListViewModel>();
        }

        private void EditTransaction(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null)
            {
                return;
            }

            (DataContext as TransactionListViewModel)?.EditCommand.Execute(transaction);
        }

        private void DeleteTransaction(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null)
            {
                return;
            }
            (DataContext as TransactionListViewModel)?.DeleteCommand.Execute(transaction);
        }

        private void OpenContextMenu(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }
    }
}