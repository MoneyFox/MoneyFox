namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;

public sealed class WelcomeViewModel(INavigationService navigationService) : NavigableViewModel
{
    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupAccountsViewModel>());
}
