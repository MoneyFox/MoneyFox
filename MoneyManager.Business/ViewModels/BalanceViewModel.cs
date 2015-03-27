#region

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

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class BalanceViewModel : ViewModelBase {
        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }
        public bool IsTransactionView { get; set; }

        public string CurrencyCulture {
            get { return settings.DefaultCurrency; }
        }

        public void UpdateBalance() {
            TotalBalance = GetTotalBalance();

            EndOfMonthBalance = GetEndOfMonthValue();
        }

        private double GetTotalBalance() {
            if (IsTransactionView) {
                return selectedAccount.CurrentBalance;
            }

            return AllAccounts != null
                ? AllAccounts.Sum(x => x.CurrentBalance)
                : 0;
        }

        private double GetEndOfMonthValue() {
            double balance = TotalBalance;
            IEnumerable<FinancialTransaction> unclearedTransactions = LoadUnclreadTransactions();

            foreach (FinancialTransaction transaction in unclearedTransactions) {
                switch (transaction.Type) {
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

        private double HandleTransferAmount(FinancialTransaction transaction, double balance) {
            if (selectedAccount == transaction.ChargedAccount) {
                balance -= transaction.Amount;
            }
            else {
                balance += transaction.Amount;
            }
            return balance;
        }

        private IEnumerable<FinancialTransaction> LoadUnclreadTransactions() {
            IEnumerable<FinancialTransaction> unclearedTransactions =
                TransactionRepository.GetUnclearedTransactions(Utilities.GetEndOfMonth());

            return IsTransactionView
                ? unclearedTransactions.Where(
                    x => x.ChargedAccount == selectedAccount || x.TargetAccount == selectedAccount).ToList()
                : unclearedTransactions;
        }

        #region Properties

        public ObservableCollection<Account> AllAccounts {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>().Data; }
        }

        private Account selectedAccount {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>().Selected; }
        }

        private ITransactionRepository TransactionRepository {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>(); }
        }

        public SettingDataAccess settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        #endregion Properties
    }
}