#region

using System;
using System.Linq;
using System.Threading.Tasks;
using BugSense;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.Business.Logic
{
    public class AccountLogic
    {
        #region Properties

        private static AccountDataAccess accountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private static TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static TransactionListViewModel transactionListView
        {
            get { return ServiceLocator.Current.GetInstance<TransactionListViewModel>(); }
        }

        #endregion Properties

        public static void PrepareAddAccount()
        {
            accountDataAccess.SelectedAccount = new Account
            {
                IsExchangeModeActive = false,
                Currency = ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency
            };
            ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit = false;
        }

        public static async void DeleteAccount(Account account, bool skipConfirmation = false)
        {
            if (skipConfirmation || await Utilities.IsDeletionConfirmed())
            {
                accountDataAccess.Delete(account);
                TransactionLogic.DeleteAssociatedTransactionsFromDatabase(account.Id);
                ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
            }
        }

        public static void RefreshRelatedTransactions()
        {
            transactionListView.SetRelatedTransactions(accountDataAccess.SelectedAccount.Id);
        }

        public static async Task RemoveTransactionAmount(FinancialTransaction transaction)
        {
            if (transaction.Cleared)
            {
                PrehandleRemoveIfTransfer(transaction);

                Func<double, double> amountFunc = x =>
                    transaction.Type == (int) TransactionType.Income
                        ? -x
                        : x;

                await HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
            }
        }

        public static async Task AddTransactionAmount(FinancialTransaction transaction)
        {
            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = x =>
                transaction.Type == (int) TransactionType.Income
                    ? x
                    : -x;

            await HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        private static async void PrehandleRemoveIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                await HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private static async Task HandleTransactionAmount(FinancialTransaction transaction,
            Func<double, double> amountFunc,
            Func<FinancialTransaction, Account> getAccountFunc)
        {
            if (transaction.ClearTransactionNow)
            {
                var account = getAccountFunc(transaction);
                if (account == null) return;

                var amountWithoutExchange = amountFunc(transaction.Amount);
                var amount = await GetAmount(amountWithoutExchange, transaction, account);

                account.CurrentBalanceWithoutExchange += amountWithoutExchange;
                account.CurrentBalance += amount;
                transaction.Cleared = true;

                accountDataAccess.Update(account);
                transactionData.Update(transaction);
            }
            else
            {
                transaction.Cleared = false;
                transactionData.Update(transaction);
            }
        }

        private static async Task<double> GetAmount(double baseAmount, FinancialTransaction transaction, Account account)
        {
            try
            {
                if (transaction.Currency != account.Currency)
                {
                    var ratio = await CurrencyLogic.GetCurrencyRatio(transaction.Currency, account.Currency);
                    return baseAmount*ratio;
                }
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
            return baseAmount;
        }

        private async static void PrehandleAddIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                await HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private static Func<FinancialTransaction, Account> GetTargetAccountFunc()
        {
            if (accountDataAccess.AllAccounts == null)
            {
                accountDataAccess.LoadList();
            }

            Func<FinancialTransaction, Account> targetAccountFunc =
                trans => accountDataAccess.AllAccounts.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private static Func<FinancialTransaction, Account> GetChargedAccountFunc()
        {
            if (accountDataAccess.AllAccounts == null)
            {
                accountDataAccess.LoadList();
            }

            Func<FinancialTransaction, Account> accountFunc =
                trans => accountDataAccess.AllAccounts.FirstOrDefault(x => x.Id == trans.ChargedAccountId);
            return accountFunc;
        }
    }
}