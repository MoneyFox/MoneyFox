using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MoneyFox.Win.ViewModels.Interfaces;
using System;

namespace MoneyFox.Win.Pages.Accounts
{
    public sealed partial class AccountListToolbar : UserControl
    {
        public IAccountListViewActionViewModel ViewModel => (IAccountListViewActionViewModel)DataContext;

        public AccountListToolbar()
        {
            InitializeComponent();
        }

        private async void AddAccountTapped(object sender, TappedRoutedEventArgs e)
        {
            var addAccountDialog = new AddAccountDialog();
            await addAccountDialog.ShowAsync();
        }
    }
}