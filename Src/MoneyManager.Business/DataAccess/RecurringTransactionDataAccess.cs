using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Business.DataAccess
{
    [ImplementPropertyChanged]
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        private readonly IDbHelper dbHelper;

        public RecurringTransactionDataAccess(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        /// <summary>
        ///     Saves an recurring transaction to the database.
        /// </summary>
        /// <param name="itemToSave">Recurring Transaction to save.</param>
        protected override void SaveToDb(RecurringTransaction itemToSave)
        {
            using (var db = dbHelper.GetSqlConnection())
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
            using (var db = dbHelper.GetSqlConnection())
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
            using (var dbConn = dbHelper.GetSqlConnection())
            {
                return dbConn.GetAllWithChildren<RecurringTransaction>().ToList();
            }
        }
    }
}