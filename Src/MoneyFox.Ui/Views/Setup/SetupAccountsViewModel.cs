namespace MoneyFox.Ui.Views.Setup;

using Common.Extensions;
using CommunityToolkit.Mvvm.Input;

public sealed class SetupAccountsViewModel : BasePageViewModel
{
    public AsyncRelayCommand GoToAddAccountCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.AddAccountRoute));

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.CategoryIntroductionRoute));

    public AsyncRelayCommand BackCommand => new(Shell.Current.Navigation.PopAsync);
}
