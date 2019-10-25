using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.DesignTime;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    ///     View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView
    {
        /// <summary>
        ///     Initialize View.
        /// </summary>
        public AccountListView()
        {
            InitializeComponent();

            if (DesignMode.DesignModeEnabled) DataContext = new DesignTimeAccountListViewModel();
        }

        private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;

            if (!(element.DataContext is AccountViewModel account)) return;

            (DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            //this has to be called before the dialog service since otherwise the data context is reseted and the account will be null
            var element = (FrameworkElement) sender;

            if (!(element.DataContext is AccountViewModel account)) return;

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.ExecuteAsync(account).FireAndForgetSafeAsync();
        }
    }
}
