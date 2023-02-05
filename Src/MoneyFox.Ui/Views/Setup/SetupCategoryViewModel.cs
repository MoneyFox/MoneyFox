namespace MoneyFox.Ui.Views.Setup;

using Common.Extensions;
using CommunityToolkit.Mvvm.Input;

internal sealed class SetupCategoryViewModel : BasePageViewModel
{
    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.SetupCompletionRoute));

    public AsyncRelayCommand BackCommand => new(Shell.Current.Navigation.PopAsync);
}
