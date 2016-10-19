using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Shared.DataAccess
{
    public class RecurringPaymentDataAccess : AbstractDataAccess<RecurringPaymentViewModel>
    {
        private readonly IDatabaseManager dbManager;

        public RecurringPaymentDataAccess(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        ///     Saves an recurring payment to the database.
        /// </summary>
        /// <param name="itemToSave">Recurring PaymentViewModel to save.</param>
        protected override void SaveToDb(RecurringPaymentViewModel itemToSave)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                //Don't use insert or replace here, because it will always replace the first element
                if (itemToSave.Id == 0)
                {
                    dbConnection.Insert(itemToSave);
                    itemToSave.Id = dbConnection.Table<RecurringPaymentViewModel>().OrderByDescending(x => x.Id).First().Id;
                }
                else
                {
                    dbConnection.Update(itemToSave);
                }
            }
        }

        /// <summary>
        ///     Deletres an recurring payment from the database.
        /// </summary>
        /// <param name="payment">recurring payment to delete.</param>
        protected override void DeleteFromDatabase(RecurringPaymentViewModel payment)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                dbConnection.Delete(payment);
            }
        }

        /// <summary>
        ///     Loads a list of recurring payments from the database filtered by the filter expression.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        /// <returns>List of loaded recurring payments.</returns>
        protected override List<RecurringPaymentViewModel> GetListFromDb(Expression<Func<RecurringPaymentViewModel, bool>> filter)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                var listQuery = dbConnection.Table<RecurringPaymentViewModel>();

                if (filter != null)
                {
                    listQuery = listQuery.Where(filter);
                }

                return listQuery.ToList();
            }
        }
    }
}