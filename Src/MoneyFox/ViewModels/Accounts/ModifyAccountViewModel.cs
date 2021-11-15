using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public abstract class ModifyAccountViewModel : ObservableRecipient
    {
        private readonly IDialogService dialogService;

        protected ModifyAccountViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public virtual bool IsEdit => false;

        private AccountViewModel selectedAccountVm = new AccountViewModel();

        /// <summary>
        ///     The currently selected CategoryViewModel
        /// </summary>
        public AccountViewModel SelectedAccountVm
        {
            get => selectedAccountVm;
            set
            {
                selectedAccountVm = value;
                OnPropertyChanged();
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
            Messenger.Send(new ReloadMessage());

            await dialogService.HideLoadingDialogAsync();
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}