using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        public TransactionDataAccess()
        {
            LoadList();
        }

        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private RecurringTransactionDataAccess RecurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        ////TODO: refactor
        //private static TransactionListUserControlViewModel TransactionListUserControlView
        //{
        //    get { return ServiceLocator.Current.GetInstance<TransactionListUserControlViewModel>(); }
        //}

        protected override void SaveToDb(FinancialTransaction transaction)
        {
            SaveToDb(transaction, false);
        }

        public void SaveToDb(FinancialTransaction transaction, bool skipRecurring)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                //TODO: refactor
                //AccountDataAccess.AddTransactionAmount(transaction);
                //if (!skipRecurring && transaction.IsRecurring)
                //{
                //    RecurringTransactionData.Save(transaction);
                //}

                AllTransactions.Add(transaction);
                AllTransactions = new ObservableCollection<FinancialTransaction>(AllTransactions.OrderBy(x => x.Date));

                //TODO: refactor
                //RefreshRelatedTransactions(transaction);

                dbConn.Insert(transaction, typeof(FinancialTransaction));
            }
        }

        //private void RefreshRelatedTransactions(FinancialTransaction transaction)
        //{
        //    if (AccountDataAccess.SelectedAccount == transaction.ChargedAccount)
        //    {
        //        //TODO: refactor
        //        //TransactionListUserControlView.SetRelatedTransactions(transaction.ChargedAccountId);
        //    }
        //}

        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                //TODO: refactor
                //AccountDataAccess.RemoveTransactionAmount(transaction);

                AllTransactions.Remove(transaction);
                //TODO: refactor
                //RefreshRelatedTransactions(transaction);
                dbConn.Delete(transaction);

                CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionData.Delete(transaction.ReccuringTransactionId.Value));
            }
        }


        protected override List<FinancialTransaction> GetListFromDb()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                return dbConn.Table<FinancialTransaction>().ToList();
            }
        }

        private void CheckIfRecurringWasRemoved(FinancialTransaction transaction)
        {
            if (!transaction.IsRecurring && transaction.ReccuringTransactionId != null)
            {
                RecurringTransactionData.Delete(transaction.ReccuringTransactionId.Value);
            }
        }

        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId)
        {
            return AllTransactions
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .ToList();
        }

        protected override async void UpdateItem(FinancialTransaction transaction)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                CheckIfRecurringWasRemoved(transaction);
                //TODO: refactor
                //AccountDataAccess.AddTransactionAmount(transaction);
                dbConn.Update(transaction);

                await CheckForRecurringTransaction(transaction, () => RecurringTransactionData.Update(transaction));
            }
        }

        private async Task CheckForRecurringTransaction(FinancialTransaction transaction,
            Action recurringTransactionAction)
        {
            //TODO: Refactor
            if (!transaction.IsRecurring) return;

            var dialog =
                new MessageDialog(Translation.GetTranslation("ChangeSubsequentTransactionsMessage"),
                    Translation.GetTranslation("ChangeSubsequentTransactionsTitle"));

            dialog.Commands.Add(new UICommand(Translation.GetTranslation("RecurringLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("JustThisLabel")));

            dialog.DefaultCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();

            if (result.Label == Translation.GetTranslation("RecurringLabel"))
            {
                recurringTransactionAction();
            }
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                return dbConn.Table<FinancialTransaction>().Where(x => x.Cleared == false
                                                                       && x.Date <= DateTime.Now).ToList();
            }
        }

        public List<FinancialTransaction> LoadRecurringList()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                //Have to make a list before apply the where statements
                return db.Table<FinancialTransaction>()
                    .ToList()
                    .Where(x => x.IsRecurring)
                    .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
                    .ToList();
            }
        }
    }
}