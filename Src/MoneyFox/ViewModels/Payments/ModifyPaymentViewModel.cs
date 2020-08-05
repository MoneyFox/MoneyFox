using GalaSoft.MvvmLight.Command;
using MoneyFox.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class ModifyPaymentViewModel : BaseViewModel
    {
        public RelayCommand GoToSelectCategoryDialogCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute));

        public RelayCommand SaveCommand => new RelayCommand(async () => await SavePaymentBaseAsync());

        private async Task SavePaymentBaseAsync()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
