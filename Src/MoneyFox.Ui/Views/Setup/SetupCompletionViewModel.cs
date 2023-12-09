namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Dashboard;

internal sealed class SetupCompletionViewModel(ISettingsFacade settingsFacade, INavigationService navigationService) : NavigableViewModel
{
    public AsyncRelayCommand CompleteCommand => new(CompleteSetup);

    public AsyncRelayCommand BackCommand => new( () => navigationService.GoBack());

    private async Task CompleteSetup()
    {
        settingsFacade.IsSetupCompleted = true;
        Application.Current!.MainPage = App.GetAppShellPage();
        navigationService.GoTo<DashboardViewModel>();
        await Shell.Current.GoToAsync(Routes.DashboardRoute);
    }
}
