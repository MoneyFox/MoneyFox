using System.Reflection;
using Windows.UI.Popups;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Models;
using MoneyManager.Src;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public ObservableCollection<FinancialTransaction> RelatedTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private RecurringTransactionDataAccess RecurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        public TransactionDataAccess()
        {
            LoadList();
        }

        protected override void SaveToDb(FinancialTransaction transaction)
        {
            SaveToDb(transaction, false);
        }

        public void SaveToDb(FinancialTransaction transaction, bool skipRecurring )
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                AccountDataAccess.AddTransactionAmount(transaction);
                if (!skipRecurring && transaction.IsRecurring)
                {
                    RecurringTransactionData.Save(transaction);
                }

                AllTransactions.Add(transaction);
                AllTransactions = new ObservableCollection<FinancialTransaction>(AllTransactions.OrderBy(x => x.Date));

                dbConn.Insert(transaction, typeof(FinancialTransaction));
            }
        }

        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AccountDataAccess.RemoveTransactionAmount(transaction);

                AllTransactions.Remove(transaction);
                RelatedTransactions.Remove(transaction);
                dbConn.Delete(transaction);

                CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionData.Delete(transaction.ReccuringTransactionId.Value));
            }
        }

        public void DeleteAssociatedTransactionsFromDatabase(int accountId)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                var transactions = dbConn.Table<FinancialTransaction>()
                    .Where(x => x.ChargedAccountId == accountId)
                    .ToList();

                foreach (var transaction in transactions)
                {
                    AllTransactions.Remove(transaction);
                    dbConn.Delete(transaction);
                }
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllTransactions = new ObservableCollection<FinancialTransaction>
                    (dbConn.Table<FinancialTransaction>().ToList());
            }
        }

        public void GetRelatedTransactions(int accountId)
        {
            RelatedTransactions = new ObservableCollection<FinancialTransaction>(
                AllTransactions
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .ToList());
        }

        protected override void UpdateItem(FinancialTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                CheckIfRecurringWasRemoved(transaction);

                AccountDataAccess.AddTransactionAmount(transaction);
                dbConn.Update(transaction);

                CheckForRecurringTransaction(transaction, () => RecurringTransactionData.Update(transaction));
            }
        }

        private void CheckIfRecurringWasRemoved(FinancialTransaction transaction)
        {
            if (!transaction.IsRecurring && transaction.ReccuringTransactionId != null)
            {
                RecurringTransactionData.Delete(transaction.ReccuringTransactionId.Value);
            }
        }

        private async void CheckForRecurringTransaction(FinancialTransaction transaction, Action recurringTransactionAction)
        {
            if (!transaction.IsRecurring) return;

            var dialog =
                new MessageDialog(Utilities.GetTranslation("ChangeSubsequentTransactionsMessage"),
                    Utilities.GetTranslation("ChangeSubsequentTransactionsTitle"));

            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("RecurringLabel")));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("JustThisLabel")));

            dialog.DefaultCommandIndex = 1;

            var result = await dialog.ShowAsync();

            if (result.Label == Utilities.GetTranslation("RecurringLabel"))
            {
                recurringTransactionAction();
            }
        }

        public void ClearTransaction()
        {
            var transactions = GetUnclearedTransactions();
            foreach (var transaction in transactions)
            {
                AccountDataAccess.AddTransactionAmount(transaction);
            }
        }

        public List<FinancialTransaction> GetUnclearedTransactions()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                return dbConn.Table<FinancialTransaction>().Where(x => x.Cleared == false
                    && x.Date >= DateTime.Now).ToList();
            }
        }

        public List<FinancialTransaction> LoadRecurringList()
        {
            using (var db = ConnectionFactory.GetDbConnection())
            {
                return db.Table<FinancialTransaction>()
                    .Where(x => x.IsRecurring).ToList();
            }
        }
    }
}