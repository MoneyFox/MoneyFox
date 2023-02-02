namespace MoneyFox.Ui.Views.SetupAssistant;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Facades;

internal sealed class SetupCompletionViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    public SetupCompletionViewModel(ISettingsFacade settingsFacade)
    {
        this.settingsFacade = settingsFacade;
    }

    public AsyncRelayCommand CompleteCommand => new(CompleteSetup);

    public AsyncRelayCommand BackCommand => new(async () => await Shell.Current.Navigation.PopAsync());

    private async Task CompleteSetup()
    {
        settingsFacade.IsSetupCompleted = true;
        await Shell.Current.GoToAsync(Routes.DashboardRoute);
    }
}
