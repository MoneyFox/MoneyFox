namespace MoneyFox.Ui.Views.Setup;

using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Queries;
using MediatR;

public sealed class SetupAccountsViewModel(INavigationService navigationService, ISender mediator) : NavigableViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoTo<AddAccountViewModel>());

    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupCategoryViewModel>());

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    public bool HasAnyAccount { get; private set; }

    public async Task MadeAccount()
    {
        var accountVms = await mediator.Send(new GetAccountsQuery());
        HasAnyAccount = accountVms.Any();
    }
}
