using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Core.DataAccess
{
    /// <summary>
    ///     Handles the access to the transaction table on the database
    /// </summary>
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        private readonly IDbHelper dbHelper;

        public TransactionDataAccess(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        /// <summary>
        ///     Saves a new item or updates an existing
        /// </summary>
        /// <param name="itemToSave">Item to Save</param>
        protected override void SaveToDb(FinancialTransaction itemToSave)
        {
            using (var db = dbHelper.GetSqlConnection())
            {
                if (itemToSave.Id == 0)
                {
                    db.Insert(itemToSave);
                    if (itemToSave.IsRecurring)
                    {
                        db.Insert(itemToSave.RecurringTransaction);
                    }
                }
                else
                {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        /// <summary>
        ///     Deletes an item from the database
        /// </summary>
        /// <param name="transaction">Item to Delete.</param>
        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (var dbConn = dbHelper.GetSqlConnection())
            {
                dbConn.Delete(transaction);
            }
        }

        /// <summary>
        ///     Loads a list of transactions from the database filtered by the expression
        /// </summary>
        /// <param name="filter">filter expression.</param>
        /// <returns>List of loaded transactions.</returns>
        protected override List<FinancialTransaction> GetListFromDb(Expression<Func<FinancialTransaction, bool>> filter)
        {
            using (var db = dbHelper.GetSqlConnection())
            {
                var list = db.GetAllWithChildren(filter, true).ToList();

                foreach (var transaction in list.Where(x => x.IsRecurring && x.ReccuringTransactionId != null))
                {
                    transaction.RecurringTransaction = db.GetWithChildren<RecurringTransaction>(transaction.ReccuringTransactionId);
                }

                return list;
            }
        }
    }
}