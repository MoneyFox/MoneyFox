namespace MoneyFox.Ui.Views.Setup;

using Common.Extensions;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Dashboard;

internal sealed class SetupCompletionViewModel(ISettingsFacade settingsFacade, INavigationService navigationService) : NavigableViewModel
{
    public AsyncRelayCommand CompleteCommand => new(CompleteSetup);

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    private Task CompleteSetup()
    {
        settingsFacade.IsSetupCompleted = true;
        Application.Current!.MainPage = GetAppShellPage();

        return navigationService.GoTo<DashboardViewModel>();
    }

    private static Page GetAppShellPage()
    {
        return DeviceInfo.Current.Idiom.UseDesktopPage() ? new AppShellDesktop() : new AppShell();
    }
}
