using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Service.Pocos;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
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

            BalanceViewModel = new DesignTimeBalanceViewModel();
        }

        public AccountViewModel SelectedAccountViewModel { get; set; }

        public ObservableCollection<AccountViewModel> IncludedAccounts { get; set; }
        public ObservableCollection<AccountViewModel> ExcludedAccounts { get; set; }
        public bool IsAllAccountsEmpty { get; set; }
        public bool IsExcludedAccountsEmpty { get; set; }
        public IBalanceViewModel BalanceViewModel { get; }
        public IViewActionViewModel ViewActionViewModel { get; }
        public MvxCommand LoadedCommand => new MvxCommand(() => { });
        public MvxAsyncCommand<AccountViewModel> EditAccountCommand => new MvxAsyncCommand<AccountViewModel>(async vm => { });
        public MvxAsyncCommand<AccountViewModel> DeleteAccountCommand => new MvxAsyncCommand<AccountViewModel>(async vm => { });
    }
}