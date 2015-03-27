
using System.Collections.Generic;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

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

        protected override List<RecurringTransaction> GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                return dbConn.Table<RecurringTransaction>().ToList();
            }
        }
    }
}