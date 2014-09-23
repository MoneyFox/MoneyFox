using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.ViewModels;
using MoneyManager.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class AccountListUserControl
    {
        public AccountListUserControl()
        {
            InitializeComponent();

            //TODO: be sure this is only called once
            ServiceLocator.Current.GetInstance<AccountDataAccess>().LoadList();
        }

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var account = element.DataContext as Account;
            if (account == null) return;

            ServiceLocator.Current.GetInstance<AccountDataAccess>().Delete(account);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var account = element.DataContext as Account;
            if (account == null) return;

            var viewModel = ServiceLocator.Current.GetInstance<AddAccountViewModel>();
            viewModel.IsEdit = true;
            viewModel.SelectedAccount = account;

            ((Frame)Window.Current.Content).Navigate(typeof(AddAccount));
        }

        private void NavigateToTransactionList(object sender, SelectionChangedEventArgs e)
        {
            if (AccountList.SelectedItem != null)
            {
                var accountId = (AccountList.SelectedItem as Account).Id;
                ServiceLocator.Current.GetInstance<TransactionDataAccess>().GetRelatedTransactions(accountId);

                ((Frame) Window.Current.Content).Navigate(typeof (TransactionList));
                AccountList.SelectedItem = null;
            }
        }
    }
}