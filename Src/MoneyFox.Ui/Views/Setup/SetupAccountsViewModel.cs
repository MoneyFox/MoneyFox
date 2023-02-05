namespace MoneyFox.Ui.Views.Setup;

using CommunityToolkit.Mvvm.Input;
using MoneyFox.Ui.Common.Extensions;

public sealed class SetupAccountsViewModel : BasePageViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.AddAccountRoute));

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.CategoryIntroductionRoute));
}
