using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.DataAccess
{
    /// <summary>
    ///     Handles the access to the Payments table on the database
    /// </summary>
    public class PaymentDataAccess : AbstractDataAccess<PaymentViewModel>
    {
        private readonly IDatabaseManager dbManager;

        public PaymentDataAccess(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        ///     Saves a new item or updates an existing
        /// </summary>
        /// <param name="itemToSave">Item to SaveItem</param>
        protected override void SaveToDb(PaymentViewModel itemToSave)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                //Don't use insert or replace here, because it will always replace the first element
                if (itemToSave.Id == 0)
                {
                    dbConnection.Insert(itemToSave);
                    itemToSave.Id = dbConnection.Table<PaymentViewModel>().OrderByDescending(x => x.Id).First().Id;
                }
                else
                {
                    dbConnection.Update(itemToSave);
                }
            }
        }

        /// <summary>
        ///     Deletes an item from the database
        /// </summary>
        /// <param name="payment">Item to Delete.</param>
        protected override void DeleteFromDatabase(PaymentViewModel payment)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                dbConnection.Delete(payment);
            }
        }

        /// <summary>
        ///     Loads a list of payments from the database filtered by the expression
        /// </summary>
        /// <param name="filter">filter expression.</param>
        /// <returns>List of loaded payments.</returns>
        protected override List<PaymentViewModel> GetListFromDb(Expression<Func<PaymentViewModel, bool>> filter)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                var listQuery = dbConnection.Table<PaymentViewModel>();

                if (filter != null)
                {
                    listQuery = listQuery.Where(filter);
                }

                var payments = listQuery.ToList();
                var accounts = dbConnection.Table<AccountViewModel>().ToList();

                var recurringTransactionsQuery = dbConnection.Table<RecurringPaymentViewModel>();
                var categoriesQuery = dbConnection.Table<CategoryViewModel>();

                foreach (var payment in payments)
                {
                    payment.ChargedAccountViewModel = accounts.FirstOrDefault(x => x.Id == payment.ChargedAccountId);
                    payment.TargetAccountViewModel = accounts.FirstOrDefault(x => x.Id == payment.TargetAccountId);

                    payment.Category = categoriesQuery.FirstOrDefault(x => x.Id == payment.CategoryId);

                    if (payment.IsRecurring)
                    {
                        payment.RecurringPayment =
                            recurringTransactionsQuery.FirstOrDefault(x => x.Id == payment.RecurringPaymentId);
                    }
                }

                return payments;
            }
        }
    }
}