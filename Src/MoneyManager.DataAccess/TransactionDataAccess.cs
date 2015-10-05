using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.DataAccess
{
    /// <summary>
    ///     Handles the access to the transaction table on the database
    /// </summary>
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        private readonly ISqliteConnectionCreator connectionCreator;

        public TransactionDataAccess(ISqliteConnectionCreator connectionCreator)
        {
            this.connectionCreator = connectionCreator;
        }

        /// <summary>
        ///     Saves a new item or updates an existing
        /// </summary>
        /// <param name="itemToSave">Item to SaveItem</param>
        protected override void SaveToDb(FinancialTransaction itemToSave)
        {
            using (var db = connectionCreator.GetConnection())
            {
                SaveRecurringTransaction(itemToSave, db);

                //Check if the transaction is new or an updated one
                if (itemToSave.Id == 0)
                {
                    db.InsertOrReplaceWithChildren(itemToSave);
                }
                else
                {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        private void SaveRecurringTransaction(FinancialTransaction itemToSave, SQLiteConnection db)
        {
            if (itemToSave.IsRecurring)
            {
                //Check if the recurring transaction is new or an updated one
                if (itemToSave.RecurringTransaction.Id == 0)
                {
                    db.Insert(itemToSave.RecurringTransaction);
                }
                else
                {
                    db.Update(itemToSave.RecurringTransaction);
                }
            }
        }

        /// <summary>
        ///     Deletes an item from the database
        /// </summary>
        /// <param name="transaction">Item to DeleteItem.</param>
        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (var dbConn = connectionCreator.GetConnection())
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
            using (var db = connectionCreator.GetConnection())
            {
                var list = db.GetAllWithChildren(filter, true).ToList();

                foreach (var transaction in list.Where(x => x.IsRecurring && x.ReccuringTransactionId != 0))
                {
                    transaction.RecurringTransaction =
                        db.GetWithChildren<RecurringTransaction>(transaction.ReccuringTransactionId);
                }

                return list;
            }
        }
    }
}