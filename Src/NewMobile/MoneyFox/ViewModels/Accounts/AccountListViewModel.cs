using System.Collections.ObjectModel;

namespace MoneyFox.ViewModels.Accounts
{
    public class AccountListViewModel : BaseViewModel
    {
        public ObservableCollection<AccountViewModel> Accounts = new ObservableCollection<AccountViewModel>
        {
            new AccountViewModel{ Name = "Income", CurrentBalance = 78542, IsExcluded = false },
            new AccountViewModel{ Name = "Expenses", CurrentBalance = 2451, IsExcluded = false },
            new AccountViewModel{ Name = "Investments", CurrentBalance = 1570, IsExcluded = true },
            new AccountViewModel{ Name = "Safety", CurrentBalance = 4455, IsExcluded = true }
        };
    }
}
