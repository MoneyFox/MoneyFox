using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Views;

namespace MoneyManager.UserControls {
    public sealed partial class AccountListUserControl {
        public AccountListUserControl() {
            InitializeComponent();
        }

        private IAccountRepository AccountRepository {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>(); }
        }

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e) {
            var senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null) {
                return;
            }

            var viewModel = ServiceLocator.Current.GetInstance<AddAccountViewModel>();
            viewModel.IsEdit = true;
            viewModel.SelectedAccount = account;

            ((Frame) Window.Current.Content).Navigate(typeof (AddAccount));
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null) {
                return;
            }

            AccountLogic.DeleteAccount(account);
        }

        private void NavigateToTransactionList(object sender, SelectionChangedEventArgs e) {
            if (AccountList.SelectedItem != null) {
                AccountRepository.Selected = AccountList.SelectedItem as Account;

                ServiceLocator.Current.GetInstance<TransactionListViewModel>()
                    .SetRelatedTransactions(AccountRepository.Selected);

                ((Frame) Window.Current.Content).Navigate(typeof (TransactionList));
                AccountList.SelectedItem = null;
            }
        }
    }
}