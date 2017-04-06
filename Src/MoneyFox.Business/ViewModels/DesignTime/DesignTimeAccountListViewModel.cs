using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public DesignTimeAccountListViewModel()
        {
            IncludedAccounts = new ObservableCollection<AccountViewModel>
            {
                new AccountViewModel
                {
                    Name = "Sparkonto",
                    CurrentBalance = 1256.25,
                    Iban = "CH12 12356 FX12 5123"
                }
            };

            BalanceViewModel = new DesignTimeBalanceViewModel();
        }

        public ObservableCollection<AccountViewModel> IncludedAccounts { get; set; }
        public ObservableCollection<AccountViewModel> ExcludedAccounts { get; set; }
        public bool IsAllAccountsEmpty { get; set; }
        public bool IsExcludedAccountsEmpty { get; set; }
        public AccountViewModel SelectedAccountViewModel { get; set; }
        public IBalanceViewModel BalanceViewModel { get; }
        public IViewActionViewModel ViewActionViewModel { get; }
        public MvxCommand LoadedCommand => new MvxCommand(() => { });
        public MvxCommand<AccountViewModel> EditAccountCommand => new MvxCommand<AccountViewModel>((vm) => { });
        public MvxCommand<AccountViewModel> DeleteAccountCommand => new MvxCommand<AccountViewModel>((vm) => { });
    }
}