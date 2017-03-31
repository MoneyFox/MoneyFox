using System;
using System.Collections.Generic;
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

        public static bool IsCacheMarkedForReload;

        private List<PaymentViewModel> DataCache { get; set; } = new List<PaymentViewModel>();

        public IEnumerable<PaymentViewModel> GetList(Expression<Func<PaymentViewModel, bool>> filter = null)
        {
            if (!DataCache.Any() || IsCacheMarkedForReload)
            {
                FillCache();
            }

            return filter != null
                ? DataCache.Where(filter.Compile()).ToList()
                : DataCache.ToList();
        }

        public PaymentViewModel FindById(int id)
        {
            if (!DataCache.Any() || IsCacheMarkedForReload)
            {
                FillCache();
            }

            return DataCache.Find(x => x.Id == id);
        }

        private void FillCache()
        {
            DataCache.Clear();
            using (var db = dbManager.GetConnection())
            {
                // Load recurring payments and mapp them to the payments to ensure recurring payments have all FK's loaded.
                var recurringPayments = db.GetAllWithChildren<RecurringPayment>();
                var payments = db.GetAllWithChildren<Payment>();

                foreach (var payment in payments.Where(x => x.IsRecurring))
                {
                    payment.RecurringPayment = recurringPayments.First(x => x.Id == payment.RecurringPaymentId);
                }

                DataCache = payments
                    .AsQueryable()
                    .ProjectTo<PaymentViewModel>()
                    .ToList();
            }
            IsCacheMarkedForReload = false;
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
                //We have to map the category ID manually, otherwise it won't be set when compiled with .net native.
                payment.CategoryId = paymentToSave.CategoryId;
                if (payment.IsRecurring)
                {
                    payment.RecurringPayment.CategoryId = paymentToSave.RecurringPayment.CategoryId;
                }

                if (payment.Id == 0)
                {
                    if (payment.IsRecurring && payment.RecurringPayment.Id == 0)
                    {
                        db.Insert(payment.RecurringPayment);
                        payment.RecurringPayment =
                            db.Table<RecurringPayment>().OrderByDescending(x => x.Id).First();
                        payment.RecurringPaymentId = payment.RecurringPayment.Id;
                    }

                    db.Insert(payment);
                    paymentToSave.Id = payment.Id;

                    SetIds(paymentToSave, payment);
                    DataCache.Add(paymentToSave);
                    return true;
                }
                db.UpdateWithChildren(payment);
                return true;
            }
        }

        private static void SetIds(PaymentViewModel paymentToSave, Payment payment)
        {
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
                DataCache.RemoveAll(x => x.Id == paymentToDelete.Id);

                var itemToDelete = db.Table<Payment>().Single(x => x.Id == paymentToDelete.Id);
                return db.Delete(itemToDelete) == 1;
            }
        }
    }
}