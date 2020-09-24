using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Facades;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class SetupCompletionViewModel : ViewModelBase
    {
        private readonly ISettingsFacade settingsFacade;

        public SetupCompletionViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public RelayCommand CompleteCommand
            => new RelayCommand(async () => await CompleteSetup());

        public RelayCommand BackCommand
            => new RelayCommand(async () => await Shell.Current.Navigation.PopAsync());

        private async Task CompleteSetup()
        {
            settingsFacade.IsSetupCompleted = true;
            Xamarin.Forms.Application.Current.MainPage = new AppShell();
            //await Shell.Current.GoToAsync($"//{ViewModelLocator.DashboardRoute}");
        }
    }
}
