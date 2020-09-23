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

        public RelayCommand CompleteCommand
            => new RelayCommand(async () => await CompleteSetup());

        public RelayCommand BackCommand
            => new RelayCommand(async () => await Shell.Current.GoToAsync(".."));

        private async Task CompleteSetup()
        {
            settingsFacade.IsSetupCompleted = true;
            await Shell.Current.GoToAsync(ViewModelLocator.DashboardRoute);
        }
    }
}
