using System.Globalization;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Models;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    internal class TotalBalanceViewModel : ViewModelBase
    {
        public ObservableCollection<Account> AllAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        public double TotalBalance { get; set; }

        public string CurrencyCulture
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        public void UpdateBalance()
        {
            TotalBalance = AllAccounts != null
                ? AllAccounts.Sum(x => x.CurrentBalance)
                : 0;
        }
    }
}