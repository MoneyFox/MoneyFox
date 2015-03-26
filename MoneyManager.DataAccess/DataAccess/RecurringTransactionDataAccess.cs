#region

using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction> {
        public ObservableCollection<RecurringTransaction> AllRecurringTransactions { get; set; }

        public RecurringTransaction SelectedRecurringTransaction { get; set; }

        protected override void SaveToDb(RecurringTransaction itemToAdd) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllRecurringTransactions == null) {
                    AllRecurringTransactions = new ObservableCollection<RecurringTransaction>();
                }

                AllRecurringTransactions.Add(itemToAdd);
                AllRecurringTransactions = new ObservableCollection<RecurringTransaction>
                    (AllRecurringTransactions.OrderBy(x => x.StartDate));

                dbConn.Insert(itemToAdd);
            }
        }

        protected override void DeleteFromDatabase(RecurringTransaction itemToDelete) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllRecurringTransactions != null) {
                    AllRecurringTransactions.Remove(itemToDelete);
                }

                dbConn.Delete(itemToDelete);
            }
        }

        public void Delete(int reccuringTransactionId) {
            RecurringTransaction recTrans = AllRecurringTransactions.FirstOrDefault(x => x.Id == reccuringTransactionId);
            if (recTrans != null) {
                Delete(recTrans);
            }
        }

        protected override void GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllRecurringTransactions =
                    new ObservableCollection<RecurringTransaction>(dbConn.Table<RecurringTransaction>().ToList());
            }
        }

        public void Save(FinancialTransaction transaction, RecurringTransaction recurringTransaction) {
            if (AllRecurringTransactions != null) {
                AllRecurringTransactions.Add(recurringTransaction);
            }

            Save(recurringTransaction);
            transaction.ReccuringTransactionId = recurringTransaction.Id;
        }

        public void Update(FinancialTransaction transaction) {
            if (!transaction.ReccuringTransactionId.HasValue) {
                Save(transaction.RecurringTransaction);
            } else {
                transaction.RecurringTransaction.Id = transaction.ReccuringTransactionId.Value;
                Save(transaction.RecurringTransaction);
            }
        }
    }
}