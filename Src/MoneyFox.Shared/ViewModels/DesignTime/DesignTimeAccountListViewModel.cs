using System.Collections.ObjectModel;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public DesignTimeAccountListViewModel()
        {
            AllAccounts = new ObservableCollection<AccountViewModel>
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

        public ObservableCollection<AccountViewModel> AllAccounts { get; set; }
        public AccountViewModel SelectedAccountViewModel { get; set; }
        public IBalanceViewModel BalanceViewModel { get; }
        public MvxCommand LoadedCommand => new MvxCommand(() => { });
    }
}