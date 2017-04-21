using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Service.Pocos;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public DesignTimeAccountListViewModel()
        {
            IncludedAccounts = new ObservableCollection<Account>
            {
                new Account
                {
                    Data =
                    {
                        Name = "Sparkonto",
                        CurrentBalance = 1256.25,
                        Iban = "CH12 12356 FX12 5123"
                    }
                }
            };

            BalanceViewModel = new DesignTimeBalanceViewModel();
        }

        public ObservableCollection<Account> IncludedAccounts { get; set; }
        public ObservableCollection<Account> ExcludedAccounts { get; set; }
        public bool IsAllAccountsEmpty { get; set; }
        public bool IsExcludedAccountsEmpty { get; set; }
        public AccountViewModel SelectedAccountViewModel { get; set; }
        public IBalanceViewModel BalanceViewModel { get; }
        public IViewActionViewModel ViewActionViewModel { get; }
        public MvxCommand LoadedCommand => new MvxCommand(() => { });
        public MvxCommand<Account> EditAccountCommand => new MvxCommand<Account>((vm) => { });
        public MvxCommand<Account> DeleteAccountCommand => new MvxCommand<Account>((vm) => { });
    }
}