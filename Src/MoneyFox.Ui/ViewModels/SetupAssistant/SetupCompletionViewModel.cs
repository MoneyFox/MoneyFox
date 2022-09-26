namespace MoneyFox.Ui.ViewModels.SetupAssistant;

using CommunityToolkit.Mvvm.Input;
using MoneyFox.Core.Common.Facades;

internal sealed class SetupCompletionViewModel : BaseViewModel
{
    private readonly ISettingsFacade settingsFacade;

    public SetupCompletionViewModel(ISettingsFacade settingsFacade)
    {
        this.settingsFacade = settingsFacade;
    }

    public RelayCommand CompleteCommand => new(CompleteSetup);

    public RelayCommand BackCommand => new(async () => await Shell.Current.Navigation.PopAsync());

    private void CompleteSetup()
    {
        settingsFacade.IsSetupCompleted = true;
        Application.Current.MainPage = new AppShell();
    }
}
