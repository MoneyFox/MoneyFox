namespace MoneyFox.Ui.Views.Setup.SetupAccounts;

using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Queries;
using MediatR;

public sealed class SetupAccountsViewModel(INavigationService navigationService, ISender mediator) : NavigableViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoTo<AddAccountViewModel>());

    public AsyncRelayCommand NextStepCommand => new(execute: () => navigationService.GoTo<SetupCategoryViewModel>(), canExecute: CheckIfAccountWasMade);

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    private bool CheckIfAccountWasMade()
    {
        var accountVms = mediator.Send(new GetAccountsQuery()).GetAwaiter().GetResult();

        return accountVms.Any();
    }
}
