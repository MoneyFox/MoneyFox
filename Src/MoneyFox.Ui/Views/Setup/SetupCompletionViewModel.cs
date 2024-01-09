namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Dashboard;

internal sealed class SetupCompletionViewModel(
    ISettingsFacade settingsFacade,
    INavigationService navigationService,
    MainPageViewModel mainPageViewModel) : NavigableViewModel
{
    public AsyncRelayCommand CompleteCommand => new(CompleteSetup);

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    private Task CompleteSetup()
    {
        settingsFacade.IsSetupCompleted = true;
        Application.Current!.MainPage = new DefaultNavigationPage(new MainPage(mainPageViewModel));

        return navigationService.NavigateFromMenuToAsync<DashboardViewModel>();
    }
}
