using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.UserControls {
    public sealed partial class AccountListUserControl {
        public AccountListUserControl() {
            InitializeComponent();
            DataContext = Mvx.Resolve<AccountListViewModel>();
        }

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e) {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e) {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null) {
                return;
            }

            (DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e) {
            //this has to be called before the dialog service since otherwise the datacontext is reseted and the account will be null
            var element = (FrameworkElement) sender;
            var account = element.DataContext as Account;
            if (account == null) {
                return;
            }

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.Execute(account);
        }
    }
}