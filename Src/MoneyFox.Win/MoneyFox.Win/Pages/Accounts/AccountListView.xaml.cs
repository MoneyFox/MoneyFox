using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MoneyFox.Win.ViewModels.Accounts;
using System;
using Windows.ApplicationModel;

namespace MoneyFox.Win.Pages.Accounts
{
    /// <summary>
    ///     View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView : BaseView
    {
        public override bool ShowHeader => false;

        private AccountListViewModel ViewModel => (AccountListViewModel)DataContext;

        /// <summary>
        ///     Initialize View.
        /// </summary>
        public AccountListView()
        {
            InitializeComponent();

            if(!DesignMode.DesignModeEnabled)
            {
                DataContext = ViewModelLocator.AccountListVm;
            }
        }

        private async void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if(!(element.DataContext is AccountViewModel account))
            {
                return;
            }

            await new EditAccountView(account.Id).ShowAsync();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            if(!(element.DataContext is AccountViewModel account))
            {
                return;
            }

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.ExecuteAsync(account);
        }

        private void AccountClicked(object sender, ItemClickEventArgs parameter)
        {
            var account = parameter.ClickedItem as AccountViewModel;
            ViewModel.OpenOverviewCommand.Execute(account);
        }
    }
}