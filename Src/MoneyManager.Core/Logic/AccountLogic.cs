using System;
using System.Linq;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Logic
{
    public class AccountLogic
    {
        public static void RefreshRelatedTransactions()
        {
            TransactionListView.SetRelatedTransactions(AccountRepository.Selected);
        }

        public static void RemoveTransactionAmount(FinancialTransaction transaction)
        {
            if (transaction.Cleared)
            {
                PrehandleRemoveIfTransfer(transaction);

                Func<double, double> amountFunc = x =>
                    transaction.Type == (int) TransactionType.Income
                        ? -x
                        : x;

                HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
            }
        }

        public static void AddTransactionAmount(FinancialTransaction transaction)
        {
            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = x =>
                transaction.Type == (int) TransactionType.Income
                    ? x
                    : -x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        private static void PrehandleRemoveIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private static void HandleTransactionAmount(FinancialTransaction transaction,
            Func<double, double> amountFunc,
            Func<FinancialTransaction, Account> getAccountFunc)
        {
            if (transaction.ClearTransactionNow)
            {
                var account = getAccountFunc(transaction);
                if (account == null)
                {
                    return;
                }

                account.CurrentBalance += amountFunc(transaction.Amount); ;
                transaction.Cleared = true;

                AccountRepository.Save(account);
                TransactionData.Save(transaction);
            }
            else
            {
                transaction.Cleared = false;
                TransactionData.Save(transaction);
            }
        }

        private static void PrehandleAddIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private static Func<FinancialTransaction, Account> GetTargetAccountFunc()
        {
            Func<FinancialTransaction, Account> targetAccountFunc =
                trans => AccountRepository.Data.FirstOrDefault(x => x.Id == trans.TargetAccount.Id);
            return targetAccountFunc;
        }

        private static Func<FinancialTransaction, Account> GetChargedAccountFunc()
        {
            Func<FinancialTransaction, Account> accountFunc =
                trans => AccountRepository.Data.FirstOrDefault(x => x.Id == trans.ChargedAccount.Id);
            return accountFunc;
        }

        #region Properties

        private static IRepository<Account> AccountRepository
            => Mvx.Resolve<IRepository<Account>>();

        private static IDataAccess<FinancialTransaction> TransactionData
            => Mvx.Resolve<IDataAccess<FinancialTransaction>>();

        private static TransactionListViewModel TransactionListView
            => Mvx.Resolve<TransactionListViewModel>();

        #endregion Properties
    }
}