using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        private readonly IMvxSqliteConnectionFactory connectionFactory;

        public RecurringTransactionDataAccess(IMvxSqliteConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        ///     Saves an recurring transaction to the database.
        /// </summary>
        /// <param name="itemToSave">Recurring Transaction to save.</param>
        protected override void SaveToDb(RecurringTransaction itemToSave)
        {
            using (var db = connectionFactory.GetConnection(Constants.DB_NAME))
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
            using (var db = connectionFactory.GetConnection(Constants.DB_NAME))
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
            using (var dbConn = connectionFactory.GetConnection(Constants.DB_NAME))
            {
                return dbConn.GetAllWithChildren<RecurringTransaction>().ToList();
            }
        }
    }
}