namespace MoneyFox.Ui.Views.Setup;

using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Queries;
using MediatR;

public sealed class SetupAccountsViewModel(INavigationService navigationService, ISender mediator) : NavigableViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoTo<AddAccountViewModel>());

    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupCategoryViewModel>(), () => HasAnyAccount);

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    public bool HasAnyAccount { get; private set; }

    public override async Task OnNavigatedAsync(object? parameter)
    {
        await CheckIfAccountWasMade();
    }

    public override async Task OnNavigatedBackAsync(object? parameter)
    {
        await CheckIfAccountWasMade();
    }

    public async Task CheckIfAccountWasMade()
    {
        var accountVms = await mediator.Send(new GetAccountsQuery());
        HasAnyAccount = accountVms.Any();
    }
}
