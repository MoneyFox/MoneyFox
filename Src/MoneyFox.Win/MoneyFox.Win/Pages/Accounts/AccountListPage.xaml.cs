namespace MoneyFox.Win.Pages.Accounts;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using ViewModels.Accounts;
using Windows.ApplicationModel;

public sealed partial class AccountListPage : BasePage
{
    public override bool ShowHeader => false;

    private AccountListViewModel ViewModel => (AccountListViewModel)DataContext;

    public AccountListPage()
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

        await new EditAccountPage(account.Id).ShowAsync();
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