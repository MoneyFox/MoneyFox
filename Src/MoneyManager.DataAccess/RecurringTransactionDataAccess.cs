using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        private readonly ISqliteConnectionCreator connectionCreator;

        public RecurringTransactionDataAccess(ISqliteConnectionCreator connectionCreator)
        {
            this.connectionCreator = connectionCreator;
        }

        /// <summary>
        ///     Saves an recurring transaction to the database.
        /// </summary>
        /// <param name="itemToSave">Recurring Transaction to save.</param>
        protected override void SaveToDb(RecurringTransaction itemToSave)
        {
            using (var db = connectionCreator.GetConnection())
            {
                if (itemToSave.Id == 0)
                {
                    db.InsertWithChildren(itemToSave);
                }
                else
                {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        /// <summary>
        ///     Deletres an recurring transaction from the database.
        /// </summary>
        /// <param name="recurringTransaction">recurring transaction to delete.</param>
        protected override void DeleteFromDatabase(RecurringTransaction recurringTransaction)
        {
            using (var db = connectionCreator.GetConnection())
            {
                db.Delete(recurringTransaction);
            }
        }

        /// <summary>
        ///     Loads a list of Recurring Transactions from the database filtered by the filter expression.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        /// <returns>List of loaded recurring transactions.</returns>
        protected override List<RecurringTransaction> GetListFromDb(Expression<Func<RecurringTransaction, bool>> filter)
        {
            using (var dbConn = connectionCreator.GetConnection())
            {
                return dbConn.GetAllWithChildren<RecurringTransaction>().ToList();
            }
        }
    }
}