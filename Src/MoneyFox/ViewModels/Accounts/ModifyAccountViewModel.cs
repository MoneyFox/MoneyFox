using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public abstract class ModifyAccountViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private readonly IDialogService dialogService;

        protected ModifyAccountViewModel(IMediator mediator,
                                         IDialogService dialogService)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
        }

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

        protected abstract Task SaveAccountAsync();

        private async Task SaveAccountBaseAsync()
        {
            if(string.IsNullOrWhiteSpace(SelectedAccountVm.Name))
            {
                await dialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if(await mediator.Send(new GetIfAccountWithNameExistsQuery(SelectedAccountVm.Name)))
            {
                await dialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
                return;
            }

            await dialogService.ShowLoadingDialogAsync(Strings.SavingAccountMessage);

            await SaveAccountAsync();
            MessengerInstance.Send(new ReloadMessage());

            await dialogService.HideLoadingDialogAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
