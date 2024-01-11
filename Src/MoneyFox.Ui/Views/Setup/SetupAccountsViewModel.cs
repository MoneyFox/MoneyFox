namespace MoneyFox.Ui.Views.Setup;

using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Core.Queries;

public sealed class SetupAccountsViewModel(INavigationService navigationService, IMediator mediator) : NavigableViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoTo<AddAccountViewModel>());

    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupCategoryViewModel>());

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    public async Task MadeAccount()
    {
        var accountVms = await mediator.Send(new GetAccountsQuery());

        HasAnyAccount = accountVms.Any();
    }

    public bool HasAnyAccount { get; private set; }
}
