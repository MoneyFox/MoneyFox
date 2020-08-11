using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class ModifyAccountViewModel : BaseViewModel
    {
        private AccountViewModel selectedAccountVm = new AccountViewModel();

        /// <summary>
        /// The currently selected CategoryViewModel
        /// </summary>
        public AccountViewModel SelectedAccountVm
        {
            get => selectedAccountVm;
            set
            {
                selectedAccountVm = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand => new RelayCommand(async () => await SaveAccountBaseAsync());

        private async Task SaveAccountBaseAsync()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
