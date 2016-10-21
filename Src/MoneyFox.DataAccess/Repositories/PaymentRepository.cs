using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using SQLiteNetExtensions.Extensions;

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
                var query = db.Table<Payment>().AsQueryable().ProjectTo<PaymentViewModel>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return query.ToList();
            }
        }

        public PaymentViewModel FindById(int id)
        {
            using (var db = dbManager.GetConnection())
            {
                try
                {
                    return Mapper.Map<PaymentViewModel>(db.GetWithChildren<Payment>(id));
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine(ex);
                    return null;
                }
            }
        }

        /// <summary>
        ///     Save a new PaymentViewModel or update an existin one.
        /// </summary>
        /// <param name="paymentToSave">item to save</param>
        /// <returns>whether the task has succeeded</returns>
        public bool Save(PaymentViewModel paymentToSave)
        {
            if (paymentToSave.ChargedAccount == null)
            {
                throw new AccountMissingException("charged accout is missing");
            }

            using (var db = dbManager.GetConnection())
            {
                paymentToSave.IsCleared = paymentToSave.ClearPaymentNow;

                var payment = Mapper.Map<Payment>(paymentToSave);

                if (payment.Id == 0)
                {
                    if (payment.IsRecurring)
                    {
                        db.Insert(payment.RecurringPayment);
                        payment.RecurringPaymentId =
                            db.Table<RecurringPayment>().OrderByDescending(x => x.Id).First().Id;
                    }

                    db.Insert(payment);
                    paymentToSave.Id = payment.Id;

                    if (paymentToSave.Category != null)
                    {
                        paymentToSave.Category.Id = payment.CategoryId ?? 0;
                    }
                    if (paymentToSave.ChargedAccount != null)
                    {
                        paymentToSave.ChargedAccount.Id = payment.ChargedAccount.Id;
                    }
                    if (paymentToSave.TargetAccount != null)
                    {
                        paymentToSave.TargetAccount.Id = payment.TargetAccount.Id;
                    }
                    if (paymentToSave.RecurringPayment != null)
                    {
                        paymentToSave.RecurringPayment.Id = payment.RecurringPaymentId;
                    }
                    return true;
                }
                db.UpdateWithChildren(payment);
                return true;
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