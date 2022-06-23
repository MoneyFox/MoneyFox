namespace MoneyFox.ViewModels.SetupAssistant
{

    using System.Threading.Tasks;
    using Common.Extensions;
    using CommunityToolkit.Mvvm.Input;
    using Core.Common.Facades;
    using Xamarin.Forms;

    internal sealed class WelcomeViewModel : BaseViewModel
    {
        private readonly ISettingsFacade settingsFacade;

        public WelcomeViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public AsyncRelayCommand GoToAddAccountCommand
            => new AsyncRelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public AsyncRelayCommand NextStepCommand
            => new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.CategoryIntroductionRoute));

        public RelayCommand SkipCommand => new RelayCommand(SkipSetup);

        public async Task InitAsync()
        {
            if (settingsFacade.IsSetupCompleted)
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
