namespace MoneyFox.Win.Pages.Accounts;

using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using ViewModels.Interfaces;

public sealed partial class AccountListToolbar : UserControl
{
    public AccountListToolbar()
    {
        InitializeComponent();
    }

    public IAccountListViewActionViewModel ViewModel => (IAccountListViewActionViewModel)DataContext;

    private async void AddAccountTapped(object sender, TappedRoutedEventArgs e)
    {
        var addAccountDialog = new AddAccountDialog();
        await addAccountDialog.ShowAsync();
    }
}
