namespace MoneyFox.ViewModels.SetupAssistant
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core._Pending_.Common.Facades;
    using Extensions;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class WelcomeViewModel : ObservableObject
    {
        private readonly ISettingsFacade settingsFacade;

        public WelcomeViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public RelayCommand GoToAddAccountCommand
            => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public RelayCommand NextStepCommand => new RelayCommand(
            async ()
                => await Shell.Current.GoToAsync(ViewModelLocator.CategoryIntroductionRoute));

        public RelayCommand SkipCommand => new RelayCommand(SkipSetup);

        public async Task InitAsync()
        {
            if(settingsFacade.IsSetupCompleted)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.DashboardRoute);
            }
        }

        private void SkipSetup()
        {
            settingsFacade.IsSetupCompleted = true;
            Application.Current.MainPage = new AppShell();
        }
    }
}