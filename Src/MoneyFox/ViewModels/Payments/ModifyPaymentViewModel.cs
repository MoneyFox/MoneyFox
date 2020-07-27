using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Payments
{
    public class ModifyPaymentViewModel : BaseViewModel
    {
        public RelayCommand SaveCommand => new RelayCommand(async () => await SavePaymentBaseAsync());

        private async Task SavePaymentBaseAsync()
        {

        }
    }
}
