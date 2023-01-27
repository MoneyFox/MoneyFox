namespace MoneyFox.Ui.Views.SetupAssistant;

using CommunityToolkit.Mvvm.Input;
using MoneyFox.Ui.Common.Extensions;
using ViewModels;

internal sealed class CategoryIntroductionViewModel : BaseViewModel
{
    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.SetupCompletionRoute));

    public AsyncRelayCommand BackCommand => new(async () => await Shell.Current.Navigation.PopAsync());
}
