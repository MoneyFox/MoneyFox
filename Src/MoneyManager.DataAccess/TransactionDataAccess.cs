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
    public class TransactionDataAccess : AbstractDataAccess<Payment>
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
        protected override void SaveToDb(Payment itemToSave)
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

        private void SaveRecurringTransaction(Payment itemToSave, SQLiteConnection db)
        {
            if (itemToSave.IsRecurring)
            {
                //Check if the recurring transaction is new or an updated one
                if (itemToSave.RecurringPayment.Id == 0)
                {
                    db.Insert(itemToSave.RecurringPayment);
                }
                else
                {
                    db.Update(itemToSave.RecurringPayment);
                }
            }
        }

        /// <summary>
        ///     Deletes an item from the database
        /// </summary>
        /// <param name="transaction">Item to Delete.</param>
        protected override void DeleteFromDatabase(Payment transaction)
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
        protected override List<Payment> GetListFromDb(Expression<Func<Payment, bool>> filter)
        {
            using (var db = connectionCreator.GetConnection())
            {
                var list = db.GetAllWithChildren(filter, true).ToList();

                foreach (var transaction in list.Where(x => x.IsRecurring && x.RecurringPaymentId != 0))
                {
                    transaction.RecurringPayment =
                        db.GetWithChildren<RecurringPayment>(transaction.RecurringPaymentId);
                }
                return list;
            }
        }
    }
}