using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces.ViewModels;

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

        public RelayCommand LoadedCommand => new RelayCommand(() => { });

        public ObservableCollection<Account> AllAccounts { get; set; }
        public Account SelectedAccount { get; set; }
        public IBalanceViewModel BalanceViewModel { get; }
    }
}