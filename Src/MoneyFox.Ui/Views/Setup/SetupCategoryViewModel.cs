namespace MoneyFox.Ui.Views.Setup;

using CommunityToolkit.Mvvm.Input;
using MoneyFox.Ui.Common.Extensions;

internal sealed class SetupCategoryViewModel : BasePageViewModel
{
    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.SetupCompletionRoute));

    public AsyncRelayCommand BackCommand => new(Shell.Current.Navigation.PopAsync);
}
