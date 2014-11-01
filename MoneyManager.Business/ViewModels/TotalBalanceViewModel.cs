using System.Collections.Generic;
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

        private Account selectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
        }

        public TransactionDataAccess TransactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public double TotalBalance { get; set; }

        public double EndOfMonthBalance { get; set; }

        public bool IsTransactionView { get; set; }

        public string CurrencyCulture
        {
            get { return CultureInfo.CurrentCulture.Name; }
        }

        public void UpdateBalance()
        {
            TotalBalance = GetTotalBalance();

            EndOfMonthBalance = GetEndOfMonthValue();
        }

        private double GetTotalBalance()
        {
            if (IsTransactionView)
            {
                return selectedAccount.CurrentBalance;
            }

            return AllAccounts != null
                ? AllAccounts.Sum(x => x.CurrentBalance)
                : 0;
        }

        private double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedTransactions = LoadUnclreadTransactions();

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

        private IEnumerable<FinancialTransaction> LoadUnclreadTransactions()
        {
            var unclearedTransactions = TransactionData.GetUnclearedTransactions();

            return IsTransactionView
                ? unclearedTransactions.Where(x => x.ChargedAccount == selectedAccount)
                : unclearedTransactions;
        }
    }
}