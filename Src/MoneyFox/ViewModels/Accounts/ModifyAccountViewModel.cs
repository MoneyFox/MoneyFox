using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Services;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public abstract class ModifyAccountViewModel : BaseViewModel
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
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
