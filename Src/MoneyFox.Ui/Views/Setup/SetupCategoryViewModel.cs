namespace MoneyFox.Ui.Views.Setup;

using Categories.ModifyCategory;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;

internal sealed class SetupCategoryViewModel(INavigationService navigationService) : NavigableViewModel
{
    public AsyncRelayCommand GoToAddCategoryCommand => new(() => navigationService.GoTo<AddCategoryViewModel>());

    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupCompletionViewModel>());

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());
}
