using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;

namespace MoneyManager.DataAccess
{
    /// <summary>
    ///     Handles the access to the Payments table on the database
    /// </summary>
    [ImplementPropertyChanged]
    public class PaymentDataAccess : AbstractDataAccess<Payment>
    {
        private readonly ISqliteConnectionCreator connectionCreator;

        public PaymentDataAccess(ISqliteConnectionCreator connectionCreator)
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
                SaveRecurringPayment(itemToSave, db);
                itemToSave.Id = db.InsertOrReplace(itemToSave);
            }
        }

        private void SaveRecurringPayment(Payment itemToSave, SQLiteConnection db)
        {
            if (itemToSave.IsRecurring)
            {
                db.InsertOrReplace(itemToSave.RecurringPayment);
            }
        }

        /// <summary>
        ///     Deletes an item from the database
        /// </summary>
        /// <param name="payment">Item to Delete.</param>
        protected override void DeleteFromDatabase(Payment payment)
        {
            using (var dbConn = connectionCreator.GetConnection())
            {
                dbConn.Delete(payment);
            }
        }

        /// <summary>
        ///     Loads a list of payments from the database filtered by the expression
        /// </summary>
        /// <param name="filter">filter expression.</param>
        /// <returns>List of loaded payments.</returns>
        protected override List<Payment> GetListFromDb(Expression<Func<Payment, bool>> filter)
        {
            using (var db = connectionCreator.GetConnection())
            {
                var listQuery = db.Table<Payment>();

                if (filter != null)
                {
                    var compiledFilter = filter.Compile();
                    listQuery = listQuery.Where(x => compiledFilter(x));
                }

                return listQuery.ToList();
            }
        }
    }
}