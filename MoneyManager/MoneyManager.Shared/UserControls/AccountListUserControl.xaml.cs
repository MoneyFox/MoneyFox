using Microsoft.Practices.ServiceLocation;
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

            ServiceLocator.Current.GetInstance<AccountListUserControlViewModel>().Delete(account);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var account = element.DataContext as Account;
            if (account == null) return;

            ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit = true;

            ((Frame)Window.Current.Content).Navigate(typeof(AddAccount));
        }
    }
}