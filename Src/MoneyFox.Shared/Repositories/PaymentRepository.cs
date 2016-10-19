using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Exceptions;

namespace MoneyFox.Shared.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDataAccess<PaymentViewModel> dataAccess;
        private List<PaymentViewModel> data;

        public PaymentRepository(IDataAccess<PaymentViewModel> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<PaymentViewModel> GetList(Expression<Func<PaymentViewModel, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            if (filter != null)
            {
                return data.Where(filter.Compile());
            }

            return data;
        }

        public PaymentViewModel FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
            return data.FirstOrDefault(p => p.Id == id);
        }


        /// <summary>
        ///     Save a new PaymentViewModel or update an existin one.
        /// </summary>
        /// <param name="payment">item to save</param>
        /// <returns>whether the task has succeeded</returns>
        public bool Save(PaymentViewModel payment)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            if (payment.ChargedAccountId == 0)
            {
                throw new AccountMissingException("charged accout is missing");
            }

            payment.IsCleared = payment.ClearPaymentNow;

            if (payment.Id == 0)
            {
                data.Add(payment);
            }

            return dataAccess.SaveItem(payment);
        }

        /// <summary>
        ///     Deletes the passed PaymentViewModel and removes the item from cache
        /// </summary>
        /// <param name="paymentToDelete">PaymentViewModel to delete.</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Delete(PaymentViewModel paymentToDelete)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            data.Remove(paymentToDelete);
            return dataAccess.DeleteItem(paymentToDelete);
        }

        /// <summary>
        ///     Loads all payments from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<PaymentViewModel, bool>> filter = null)
        {
            data = dataAccess.LoadList();
        }
    }
}