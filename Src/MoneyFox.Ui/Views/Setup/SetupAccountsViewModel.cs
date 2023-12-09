namespace MoneyFox.Ui.Views.Setup;

using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;

public sealed class SetupAccountsViewModel(INavigationService navigationService) : BasePageViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoBack<AddAccountViewModel>());

    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupCategoryViewModel>());

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());
}
