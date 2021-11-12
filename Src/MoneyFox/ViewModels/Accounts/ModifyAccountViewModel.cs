using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public abstract class ModifyAccountViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        private AccountViewModel selectedAccountVm = new AccountViewModel();

        protected ModifyAccountViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public virtual bool IsEdit => false;

        /// <summary>
        ///     The currently selected CategoryViewModel
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

        protected abstract Task SaveAccountAsync();

        private async Task SaveAccountBaseAsync()
        {
            if(string.IsNullOrWhiteSpace(SelectedAccountVm.Name))
            {
                await dialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await dialogService.ShowLoadingDialogAsync(Strings.SavingAccountMessage);

            await SaveAccountAsync();
            MessengerInstance.Send(new ReloadMessage());

            await dialogService.HideLoadingDialogAsync();
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}