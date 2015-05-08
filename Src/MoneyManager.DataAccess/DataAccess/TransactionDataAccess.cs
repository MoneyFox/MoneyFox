using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.DataAccess.DataAccess {
    /// <summary>
    /// Handles the access to the transaction table on the database
    /// </summary>
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction> {
        /// <summary>
        /// Saves a new item or updates an existing
        /// </summary>
        /// <param name="itemToSave">Item to Save</param>
        protected override void SaveToDb(FinancialTransaction itemToSave) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (itemToSave.Id == 0) {
                    db.Insert(itemToSave);
                }
                else {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        /// <summary>
        /// Deletes an item from the database
        /// </summary>
        /// <param name="transaction">Item to Delete.</param>
        protected override void DeleteFromDatabase(FinancialTransaction transaction) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                dbConn.Delete(transaction);
            }
        }

        /// <summary>
        ///     Loads a list of transactions from the database filtered by the expression
        /// </summary>
        /// <param name="filter">filter expression.</param>
        /// <returns>List of loaded transactions.</returns>
        protected override List<FinancialTransaction> GetListFromDb(Expression<Func<FinancialTransaction, bool>> filter) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                return db.GetAllWithChildren(filter, true).ToList();
            }
        }
    }
}