using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyTracker.Models;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class AccountViewModel : ViewModelBase
    {
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }
    }
}