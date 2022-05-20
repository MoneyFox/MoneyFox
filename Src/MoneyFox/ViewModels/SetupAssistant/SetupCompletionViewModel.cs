namespace MoneyFox.ViewModels.SetupAssistant
{

    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core._Pending_.Common.Facades;

    public class SetupCompletionViewModel : ObservableObject
    {
        private readonly ISettingsFacade settingsFacade;

        public SetupCompletionViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public RelayCommand CompleteCommand => new RelayCommand(CompleteSetup);

        public RelayCommand BackCommand => new RelayCommand(async () => await Shell.Current.Navigation.PopAsync());

        private void CompleteSetup()
        {
            settingsFacade.IsSetupCompleted = true;
            Application.Current.MainPage = new AppShell();
        }
    }

}
