namespace MoneyFox.Ui.Views.SetupAssistant;

using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Facades;

internal sealed class WelcomeViewModel : BaseViewModel
{
    private readonly ISettingsFacade settingsFacade;

    public WelcomeViewModel(ISettingsFacade settingsFacade)
    {
        this.settingsFacade = settingsFacade;
    }

    public AsyncRelayCommand GoToAddAccountCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.AddAccountRoute));

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.CategoryIntroductionRoute));

    public RelayCommand SkipCommand => new(SkipSetup);

    public async Task InitAsync()
    {
        if (settingsFacade.IsSetupCompleted)
        {
            await Shell.Current.GoToAsync(Routes.DashboardRoute);
        }
    }

    private void SkipSetup()
    {
        settingsFacade.IsSetupCompleted = true;
        Application.Current.MainPage = new AppShell();
    }
}
