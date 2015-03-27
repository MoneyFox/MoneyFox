#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction> {
        protected override void SaveToDb(RecurringTransaction itemToSave) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (itemToSave.Id == 0) {
                    db.InsertWithChildren(itemToSave);
                } else {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        protected override void DeleteFromDatabase(RecurringTransaction itemToDelete) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                db.Delete(itemToDelete);
            }
        }

        public void Delete(int reccuringTransactionId) {
            //RecurringTransaction recTrans = AllRecurringTransactions.FirstOrDefault(x => x.Id == reccuringTransactionId);
            //if (recTrans != null) {
            //    Delete(recTrans);
            //}
        }

        protected override List<RecurringTransaction> GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                return dbConn.Table<RecurringTransaction>().ToList();
            }
        }

        public void Save(FinancialTransaction transaction, RecurringTransaction recurringTransaction) {
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