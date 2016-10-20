using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;

namespace MoneyFox.DataAccess.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDatabaseManager dbManager;

        /// <summary>
        ///     Creates a PaymentRepository Object
        /// </summary>
        /// <param name="dbManager">Instanced <see cref="IDatabaseManager"/> data Access</param>
        public PaymentRepository(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public IEnumerable<PaymentViewModel> GetList(Expression<Func<PaymentViewModel, bool>> filter = null)
        {
            using (var db = dbManager.GetConnection())
            {
                return Mapper.Map<List<PaymentViewModel>>(db.Table<Payment>());
            }
        }

        public PaymentViewModel FindById(int id)
        {
            using (var db = dbManager.GetConnection())
            {
                return Mapper.Map<PaymentViewModel>(db.Table<Payment>().FirstOrDefault(x => x.Id == id));
            }
        }


        /// <summary>
        ///     Save a new PaymentViewModel or update an existin one.
        /// </summary>
        /// <param name="paymentToSave">item to save</param>
        /// <returns>whether the task has succeeded</returns>
        public bool Save(PaymentViewModel paymentToSave)
        {
            if (paymentToSave.ChargedAccountId == 0)
            {
                throw new AccountMissingException("charged accout is missing");
            }

            using (var db = dbManager.GetConnection())
            {
                paymentToSave.IsCleared = paymentToSave.ClearPaymentNow;

                var payment = Mapper.Map<Payment>(paymentToSave);

                if (payment.Id == 0)
                {
                    var rows = db.Insert(payment);
                    payment.Id = db.Table<Payment>().OrderByDescending(x => x.Id).First().Id;
                    return rows == 1;
                }
                return db.Update(payment) == 1;

            }
        }

        /// <summary>
        ///     Deletes the passed PaymentViewModel and removes the item from cache
        /// </summary>
        /// <param name="paymentToDelete">PaymentViewModel to delete.</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Delete(PaymentViewModel paymentToDelete)
        {
            using (var db = dbManager.GetConnection())
            {
                var itemToDelete = db.Table<Payment>().Single(x => x.Id == paymentToDelete.Id);
                return db.Delete(itemToDelete) == 1;
            }
        }
    }
}