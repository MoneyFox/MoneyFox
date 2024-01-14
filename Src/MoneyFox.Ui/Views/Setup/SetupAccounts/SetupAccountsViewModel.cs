namespace MoneyFox.Ui.Views.Setup.SetupAccounts;

using System.Collections.ObjectModel;
using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Extensions;
using Core.Queries;
using MediatR;

public sealed class SetupAccountsViewModel(INavigationService navigationService, ISender mediator) : NavigableViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoTo<AddAccountViewModel>());

    public AsyncRelayCommand NextStepCommand => new(execute: () => navigationService.GoTo<SetupCategoryViewModel>(), canExecute: () => IsNextStepAvailable);

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    public ObservableCollection<AccountViewModel> ExistingAccounts { get; } = [];

    public bool IsNextStepAvailable => ExistingAccounts.Count > 0;

    public override Task OnNavigatedAsync(object? parameter)
    {
        return LoadExistingAccounts();
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return LoadExistingAccounts();
    }

    private async Task LoadExistingAccounts()
    {
        var accounts = await mediator.Send(new GetAccountsQuery());
        ExistingAccounts.Clear();
        ExistingAccounts.AddRange(accounts.Select(a => new AccountViewModel(Id: a.Id, Name: a.Name)));
        OnPropertyChanged(nameof(IsNextStepAvailable));
    }
}

public record AccountViewModel(int Id, string Name);
