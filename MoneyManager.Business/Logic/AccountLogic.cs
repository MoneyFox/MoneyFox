using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Manager;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Xamarin;

namespace MoneyManager.Business.Logic {
    public class AccountLogic {
        #region Properties

        private static IAccountRepository AccountRepository {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>(); }
        }

        private static IDataAccess<FinancialTransaction> transactionData {
            get { return ServiceLocator.Current.GetInstance<IDataAccess<FinancialTransaction>>(); }
        }

        private static TransactionListViewModel transactionListView {
            get { return ServiceLocator.Current.GetInstance<TransactionListViewModel>(); }
        }
        private static CurrencyManager currencyManager {
            get { return ServiceLocator.Current.GetInstance<CurrencyManager>(); }
        }

        #endregion Properties

        public static void PrepareAddAccount() {
            AccountRepository.Selected = new Account {
                IsExchangeModeActive = false,
                Currency = ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency
            };
            ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit = false;
        }

        public static async void DeleteAccount(Account account, bool skipConfirmation = false) {
            if (skipConfirmation || await Utilities.IsDeletionConfirmed()) {
                AccountRepository.Delete(account);
                TransactionLogic.DeleteAssociatedTransactionsFromDatabase(account);
                ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
            }
        }

        public static void RefreshRelatedTransactions() {
            transactionListView.SetRelatedTransactions(AccountRepository.Selected);
        }

        public static async Task RemoveTransactionAmount(FinancialTransaction transaction) {
            if (transaction.Cleared) {
                PrehandleRemoveIfTransfer(transaction);

                Func<double, double> amountFunc = x =>
                    transaction.Type == (int) TransactionType.Income
                        ? -x
                        : x;

                await HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
            }
        }

        public static async Task AddTransactionAmount(FinancialTransaction transaction) {
            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = x =>
                transaction.Type == (int) TransactionType.Income
                    ? x
                    : -x;

            await HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        private static async void PrehandleRemoveIfTransfer(FinancialTransaction transaction) {
            if (transaction.Type == (int) TransactionType.Transfer) {
                Func<double, double> amountFunc = x => -x;
                await HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private static async Task HandleTransactionAmount(FinancialTransaction transaction,
            Func<double, double> amountFunc,
            Func<FinancialTransaction, Account> getAccountFunc) {
            if (transaction.ClearTransactionNow) {
                Account account = getAccountFunc(transaction);
                if (account == null) {
                    return;
                }

                double amountWithoutExchange = amountFunc(transaction.Amount);
                double amount = await GetAmount(amountWithoutExchange, transaction, account);

                account.CurrentBalanceWithoutExchange += amountWithoutExchange;
                account.CurrentBalance += amount;
                transaction.Cleared = true;

                AccountRepository.Save(account);
                transactionData.Save(transaction);
            }
            else {
                transaction.Cleared = false;
                transactionData.Save(transaction);
            }
        }

        private static async Task<double> GetAmount(double baseAmount, FinancialTransaction transaction, Account account) {
            try {
                if (transaction.Currency != account.Currency) {
                    double ratio = await currencyManager.GetCurrencyRatio(transaction.Currency, account.Currency);
                    return baseAmount*ratio;
                }
            }
            catch (Exception ex) {
                Insights.Report(ex, ReportSeverity.Error);
            }
            return baseAmount;
        }

        private static async void PrehandleAddIfTransfer(FinancialTransaction transaction) {
            if (transaction.Type == (int) TransactionType.Transfer) {
                Func<double, double> amountFunc = x => x;
                await HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private static Func<FinancialTransaction, Account> GetTargetAccountFunc() {
            Func<FinancialTransaction, Account> targetAccountFunc =
                trans => AccountRepository.Data.FirstOrDefault(x => x.Id == trans.TargetAccount.Id);
            return targetAccountFunc;
        }

        private static Func<FinancialTransaction, Account> GetChargedAccountFunc() {
            Func<FinancialTransaction, Account> accountFunc =
                trans => AccountRepository.Data.FirstOrDefault(x => x.Id == trans.ChargedAccount.Id);
            return accountFunc;
        }
    }
}