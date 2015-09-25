using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class AccountListUserControl
    {
        public AccountListUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<AccountListViewModel>();
        }

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
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

            (DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            //this has to be called before the dialog service since otherwise the datacontext is reseted and the account will be null
            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null)
            {
                return;
            }

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.Execute(account);
        }
    }
}