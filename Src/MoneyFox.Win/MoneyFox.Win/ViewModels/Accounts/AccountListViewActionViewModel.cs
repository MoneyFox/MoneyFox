﻿namespace MoneyFox.Win.ViewModels.Accounts;

using CommunityToolkit.Mvvm.Input;
using Interfaces;
using Services;

internal sealed class AccountListViewActionViewModel : BaseViewModel, IAccountListViewActionViewModel
{
    private readonly INavigationService navigationService;

    public AccountListViewActionViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public RelayCommand GoToAddAccountCommand => new(() => navigationService.Navigate<AddAccountViewModel>());
}
