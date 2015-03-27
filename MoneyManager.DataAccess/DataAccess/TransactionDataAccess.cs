#region

using System;
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
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction> {
       protected override void SaveToDb(FinancialTransaction itemToSave) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (itemToSave.Id == 0) {
                    db.InsertWithChildren(itemToSave);
                } else {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        protected override void DeleteFromDatabase(FinancialTransaction transaction) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                dbConn.Delete(transaction);
            }
        }

        protected override List<FinancialTransaction> GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                return dbConn.Table<FinancialTransaction>().ToList();
            }
        }




        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId) {
            //if (AllTransactions == null) {
            //    LoadList();
            //}

            //return AllTransactions
            //    .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
            //    .OrderByDescending(x => x.Date)
            //    .ToList();
            return new List<FinancialTransaction>();
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions() {
            return GetUnclearedTransactions(DateTime.Today);
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date) {
            //if (AllTransactions == null) {
            //    LoadList();
            //}

            //return AllTransactions.Where(x => x.Cleared == false
            //                                  && x.Date.Date <= date.Date).ToList();
            return new List<FinancialTransaction>();
        }

        public List<FinancialTransaction> LoadRecurringList() {
            //if (AllTransactions == null) {
            //    LoadList();
            //}

            //return AllTransactions
            //    .Where(x => x.IsRecurring)
            //    .Where(x => x.RecurringTransaction != null)
            //    .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
            //    .ToList();
            return new List<FinancialTransaction>();
        }
    }
}