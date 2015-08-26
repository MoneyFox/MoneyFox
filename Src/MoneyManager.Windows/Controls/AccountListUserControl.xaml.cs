using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Windows.Views;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class AccountListUserControl
    {
        public AccountListUserControl()
        {
            InitializeComponent();
        }

        private IRepository<Account> AccountRepository => Mvx.Resolve<IRepository<Account>>();

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null)
            {
                return;
            }
            
            //TODO refactor this / move to a viewmodel
            var viewModel = Mvx.Resolve<AddAccountViewModel>();
            viewModel.IsEdit = true;
            viewModel.SelectedAccount = account;

            ((Frame) Window.Current.Content).Navigate(typeof (AddAccountView));
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: refactor this
            //if (!await Utilities.IsDeletionConfirmed())
            //{
            //    return;
            //}

            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null)
            {
                return;
            }

            //TODO Refactor
            Mvx.Resolve<AccountListUserControlViewModel>().Delete(account);
            Mvx.Resolve<BalanceViewModel>().UpdateBalance();
        }

        private void NavigateToTransactionList(object sender, SelectionChangedEventArgs e)
        {
            if (AccountList.SelectedItem != null)
            {
                AccountRepository.Selected = AccountList.SelectedItem as Account;

                Mvx.Resolve<TransactionListViewModel>()
                    .SetRelatedTransactions(AccountRepository.Selected);

                //TODO move toviewmodel
                ((Frame) Window.Current.Content).Navigate(typeof (TransactionListView));
                AccountList.SelectedItem = null;
            }
        }
    }
}