using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Extensions;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.SetupAssistant
{
    public class WelcomeViewModel : ViewModelBase
    {
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
