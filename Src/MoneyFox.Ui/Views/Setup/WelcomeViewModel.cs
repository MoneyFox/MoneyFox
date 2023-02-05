namespace MoneyFox.Ui.Views.Setup;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;

internal sealed class WelcomeViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    public WelcomeViewModel(ISettingsFacade settingsFacade)
    {
        this.settingsFacade = settingsFacade;
    }

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.CurrencyIntroductionRoute));

    public async Task InitAsync()
    {
        if (settingsFacade.IsSetupCompleted)
        {
            await Shell.Current.GoToAsync(Routes.DashboardRoute);
        }
    }
}
