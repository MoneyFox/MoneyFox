using System.Collections.ObjectModel;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public DesignTimeAccountListViewModel()
        {
            AllAccounts = new ObservableCollection<Account>
            {
                new Account
                {
                    Name = "Sparkonto",
                    CurrentBalance = 1256.25,
                    Iban = "CH12 12356 FX12 5123"
                }
            };

            BalanceViewModel = new DesignTimeBalanceViewModel();
        }

        public ObservableCollection<Account> AllAccounts { get; set; }
        public Account SelectedAccount { get; set; }
        public IBalanceViewModel BalanceViewModel { get; }
        public MvxCommand LoadedCommand => new MvxCommand(() => { });
    }
}
