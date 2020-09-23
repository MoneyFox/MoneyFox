using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.SetupAssistant
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly ISettingsFacade settingsFacade;

        public WelcomeViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public async Task InitAsync()
        {
            if(settingsFacade.IsSetupCompleted)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.DashboardRoute);
            }
        }

        public RelayCommand GoToAddAccountCommand
            => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public RelayCommand NextStepCommand => new RelayCommand(async ()
            => await Shell.Current.GoToAsync(ViewModelLocator.AddPaymentRoute));
        public RelayCommand SkipCommand => new RelayCommand(SkipSetup);

        private void SkipSetup()
        {

        }
    }
}
