using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Extensions;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class SetupCompletionViewModel : ViewModelBase
    {
        public RelayCommand CompleteCommand
            => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.DashboardRoute));

        public RelayCommand BackCommand
            => new RelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }
}
