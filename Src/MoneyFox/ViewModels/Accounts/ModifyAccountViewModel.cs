using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.Messages;
using MoneyFox.Core.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Accounts
{
    public abstract class ModifyAccountViewModel : ObservableRecipient
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
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}