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
        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

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
                AllTransactions.Remove(transaction);
                dbConn.Delete(transaction);
            }
        }

        protected override void GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllTransactions =
                    new ObservableCollection<FinancialTransaction>(dbConn.Table<FinancialTransaction>().ToList());
            }
        }

        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId) {
            if (AllTransactions == null) {
                LoadList();
            }

            return AllTransactions
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions() {
            return GetUnclearedTransactions(DateTime.Today);
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date) {
            if (AllTransactions == null) {
                LoadList();
            }

            return AllTransactions.Where(x => x.Cleared == false
                                              && x.Date.Date <= date.Date).ToList();
        }

        public List<FinancialTransaction> LoadRecurringList() {
            if (AllTransactions == null) {
                LoadList();
            }

            return AllTransactions
                .Where(x => x.IsRecurring)
                .Where(x => x.RecurringTransaction != null)
                .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
                .ToList();
        }
    }
}