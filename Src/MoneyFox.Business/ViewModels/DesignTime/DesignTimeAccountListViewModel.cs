using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.Pocos;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    /// <inheritdoc />
    public class DesignTimeAccountListViewModel : MvxViewModel, IAccountListViewModel
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DesignTimeAccountListViewModel()
        {
            IncludedAccounts = new ObservableCollection<AccountViewModel>
            {
                new AccountViewModel(new Account())
                {
                    Name = "Sparkonto",
                    CurrentBalance = 1256.25,
                    Iban = "CH12 12356 FX12 5123"
                }
            };

            ViewActionViewModel = new AccountListViewActionViewModel(null, null);
            BalanceViewModel = new DesignTimeBalanceViewModel();
        }

        /// <inheritdoc />
        public AccountViewModel SelectedAccountViewModel { get; set; }

        /// <inheritdoc />
        public ObservableCollection<AccountViewModel> IncludedAccounts { get; set; }

        /// <inheritdoc />
        public ObservableCollection<AccountViewModel> ExcludedAccounts { get; set; }

        /// <inheritdoc />
        public bool IsAllAccountsEmpty { get; set; }

        /// <inheritdoc />
        public bool IsExcludedAccountsEmpty { get; set; }

        /// <inheritdoc />
        public IBalanceViewModel BalanceViewModel { get; }

        /// <inheritdoc />
        public IViewActionViewModel ViewActionViewModel { get; }

        /// <inheritdoc />
        public IMvxLanguageBinder TextSource { get; }

        /// <inheritdoc />
        public MvxAsyncCommand LoadedCommand => new MvxAsyncCommand(() => Task.CompletedTask);

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> OpenOverviewCommand => new MvxAsyncCommand<AccountViewModel>(async vm => { await Task.CompletedTask; });

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> EditAccountCommand =>
            new MvxAsyncCommand<AccountViewModel>(async vm => { await Task.CompletedTask; });

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> DeleteAccountCommand =>
            new MvxAsyncCommand<AccountViewModel>(async vm => { await Task.CompletedTask; });

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddAccountCommand => new MvxAsyncCommand(async vm => { await Task.CompletedTask; });
        
        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> OpenContextMenuCommand => new MvxAsyncCommand<AccountViewModel>(async vm => { await Task.CompletedTask; });
        
        /// <inheritdoc />
        public Task ShowMenu()
        {
            return Task.CompletedTask;
        }
    }
}