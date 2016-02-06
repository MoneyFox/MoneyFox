using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class RecurringPaymentDataAccess : AbstractDataAccess<RecurringPayment>
    {
        private readonly ISqliteConnectionCreator connectionCreator;

        public RecurringPaymentDataAccess(ISqliteConnectionCreator connectionCreator)
        {
            this.connectionCreator = connectionCreator;
        }

        /// <summary>
        ///     Saves an recurring payment to the database.
        /// </summary>
        /// <param name="itemToSave">Recurring Payment to save.</param>
        protected override void SaveToDb(RecurringPayment itemToSave)
        {
            using (var db = connectionCreator.GetConnection())
            {
                itemToSave.Id = db.InsertOrReplace(itemToSave);
            }
        }

        /// <summary>
        ///     Deletres an recurring payment from the database.
        /// </summary>
        /// <param name="payment">recurring payment to delete.</param>
        protected override void DeleteFromDatabase(RecurringPayment payment)
        {
            using (var db = connectionCreator.GetConnection())
            {
                db.Delete(payment);
            }
        }

        /// <summary>
        ///     Loads a list of recurring payments from the database filtered by the filter expression.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        /// <returns>List of loaded recurring payments.</returns>
        protected override List<RecurringPayment> GetListFromDb(Expression<Func<RecurringPayment, bool>> filter)
        {
            using (var db = connectionCreator.GetConnection())
            {
                var listQuery = db.Table<RecurringPayment>();

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