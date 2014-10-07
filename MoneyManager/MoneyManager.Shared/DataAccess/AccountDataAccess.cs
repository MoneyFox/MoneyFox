using System;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account>
    {
        private TransactionDataAccess TransactionViewModel
        {
            get { return new TransactionDataAccess(); }
        }

        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllAccounts == null)
                {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(account);
                account.Id = dbConn.Insert(account);
            }
        }

        protected override void DeleteFromDatabase(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                TransactionViewModel.DeleteAssociatedTransactionsFromDatabase(account.Id);

                AllAccounts.Remove(account);
                dbConn.Delete(account);
            }
        }

        protected override sealed void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>().ToList());
                ServiceLocator.Current.GetInstance<TotalBalanceViewModel>().UpdateBalance();
            }
        }

        protected override void UpdateItem(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(account, typeof(Account));
            }
        }

        public void RemoveTransactionAmount(FinancialTransaction transaction)
        {
            PrehandleRemoveIfTransfer(transaction);

            Func<double, double> amountFunc = (x) =>
                transaction.Type == (int) TransactionType.Income
                    ? -x
                    : x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        public void AddTransactionAmount(FinancialTransaction transaction)
        {
            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = (x) =>
                transaction.Type == (int) TransactionType.Income
                    ? x
                    : -x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        private void PrehandleAddIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private void PrehandleRemoveIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int)TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<FinancialTransaction, Account> GetTargetAccountFunc()
        {
            Func<FinancialTransaction, Account> targetAccountFunc =
                (trans) => AllAccounts.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private Func<FinancialTransaction, Account> GetChargedAccountFunc()
        {
            Func<FinancialTransaction, Account> accountFunc =
                (trans) => AllAccounts.FirstOrDefault(x => x.Id == trans.ChargedAccountId);
            return accountFunc;
        }

        private void HandleTransactionAmount(FinancialTransaction transaction, Func<double, double> amountFunc, Func<FinancialTransaction, Account> getAccountFunc)
        {
            if (transaction.ClearTransactionNow)
            {
                var account = getAccountFunc(transaction); 
                if (account == null) return;

                var amount = amountFunc(transaction.Amount);
                
                account.CurrentBalance += amount;
                transaction.Cleared = true;
                UpdateItem(account);
            }
        }
    }
}