using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using System;
using System.Linq;

namespace MoneyManager.Business
{
    //TODO: Refactor
    internal class AccountLogic
    {
        private AccountDataAccess accountData
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public void RemoveTransactionAmount(FinancialTransaction transaction, Account account)
        {
            PrehandleRemoveIfTransfer(transaction);

            Func<double, double> amountFunc = (x) =>
                transaction.Type == (int)TransactionType.Income
                    ? -x
                    : x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        public void AddTransactionAmount(FinancialTransaction transaction)
        {
            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = (x) =>
                transaction.Type == (int)TransactionType.Income
                    ? x
                    : -x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }


        private void PrehandleRemoveIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int)TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
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
                accountData.Update(account);
            }
        }


        private void PrehandleAddIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int)TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<FinancialTransaction, Account> GetTargetAccountFunc()
        {
            Func<FinancialTransaction, Account> targetAccountFunc =
                (trans) => accountData.AllAccounts.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private Func<FinancialTransaction, Account> GetChargedAccountFunc()
        {
            Func<FinancialTransaction, Account> accountFunc =
                (trans) => accountData.AllAccounts.FirstOrDefault(x => x.Id == trans.ChargedAccountId);
            return accountFunc;
        }
    }
}