using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.ViewModels
{
    public class TotalBalanceViewModel : ViewModelBase
    {
        public SettingDataAccess Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        public ObservableCollection<Account> AllAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        public double TotalBalance
        {
            get
            {
                return AllAccounts != null ?
                    AllAccounts.Sum(x => x.CurrentBalance)
                    : 0;
            }
        }
    }
}
