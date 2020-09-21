using MoneyFox.Application.Common;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.DesignTime;
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.Accounts
{
    /// <summary>
    /// View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView
    {
        public override bool ShowHeader => false;

        /// <summary>
        /// Initialize View.
        /// </summary>
        public AccountListView()
        {
            InitializeComponent();

            if(DesignMode.DesignModeEnabled)
            {
                DataContext = new DesignTimeAccountListViewModel();
            }
        }

        private async void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            if(!(element.DataContext is AccountViewModel account)) return;

            await new EditAccountView(account.Id).ShowAsync();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;

            if(!(element.DataContext is AccountViewModel account))
                return;

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.ExecuteAsync(account).FireAndForgetSafeAsync();
        }

        private void AccountClicked(object sender, ItemClickEventArgs parameter)
        {
            var account = parameter.ClickedItem as AccountViewModel;
            (DataContext as AccountListViewModel)?.OpenOverviewCommand.Execute(account);
        }
    }
}
