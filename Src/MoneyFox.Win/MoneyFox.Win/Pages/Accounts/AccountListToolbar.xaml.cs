namespace MoneyFox.Win.Pages.Accounts;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using ViewModels.Interfaces;

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