using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class TotalBalanceViewModel : ViewModelBase
    {
        public ObservableCollection<Account> AllAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        public TransactionDataAccess TransactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }

        public string CurrencyCulture
        {
            get { return CultureInfo.CurrentCulture.Name; }
        }

        public void UpdateBalance()
        {
            TotalBalance = AllAccounts != null
                ? AllAccounts.Sum(x => x.CurrentBalance)
                : 0;

            EndOfMonthBalance = GetEndOfMonthValue();
        }

        private double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedTransactions = TransactionData.GetUnclearedTransactions();

            foreach (var transaction in unclearedTransactions)
            {
                switch (transaction.Type)
                {
                    case (int) TransactionType.Spending:
                        balance -= transaction.Amount;
                        break;
                    case (int) TransactionType.Income:
                        balance += transaction.Amount;
                        break;
                }
            }

            return balance;
        }
    }
}