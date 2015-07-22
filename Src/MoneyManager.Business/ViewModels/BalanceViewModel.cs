using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class BalanceViewModel : ViewModelBase
    {
        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }
        public bool IsTransactionView { private get; set; }
        public string CurrencyCulture => settings.DefaultCurrency;

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

            return AllAccounts?.Sum(x => x.CurrentBalance) ?? 0;
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

                    case (int) TransactionType.Transfer:
                        balance = HandleTransferAmount(transaction, balance);
                        break;
                }
            }

            return balance;
        }

        private double HandleTransferAmount(FinancialTransaction transaction, double balance)
        {
            if (selectedAccount == transaction.ChargedAccount)
            {
                balance -= transaction.Amount;
            }
            else
            {
                balance += transaction.Amount;
            }
            return balance;
        }

        private IEnumerable<FinancialTransaction> LoadUnclreadTransactions()
        {
            var unclearedTransactions =
                TransactionRepository.GetUnclearedTransactions(Utilities.GetEndOfMonth());

            return IsTransactionView
                ? unclearedTransactions.Where(
                    x => x.ChargedAccountId == selectedAccount.Id || x.TargetAccountId == selectedAccount.Id).ToList()
                : unclearedTransactions;
        }

        #region Properties

        private ObservableCollection<Account> AllAccounts
            => ServiceLocator.Current.GetInstance<IRepository<Account>>().Data;

        private Account selectedAccount => ServiceLocator.Current.GetInstance<IRepository<Account>>().Selected;

        private ITransactionRepository TransactionRepository
            => ServiceLocator.Current.GetInstance<ITransactionRepository>();

        private SettingDataAccess settings => ServiceLocator.Current.GetInstance<SettingDataAccess>();

        #endregion Properties
    }
}