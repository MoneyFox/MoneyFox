using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class ModifyAccountViewModel : BaseViewModel
    {
        public RelayCommand SaveCommand => new RelayCommand(async () => await SaveAccountBaseAsync());

        private async Task SaveAccountBaseAsync()
        {

        }
    }
}
