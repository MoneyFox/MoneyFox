using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories
{
    [ImplementPropertyChanged]
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDataAccess<Payment> dataAccess;
        private List<Payment> data;

        public PaymentRepository(IDataAccess<Payment> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<Payment> GetList(Expression<Func<Payment, bool>> filter = null)
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

        public Payment FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
            return data.FirstOrDefault(p => p.Id == id);
        }


        /// <summary>
        ///     The currently selected Payment
        /// </summary>
        public Payment Selected { get; set; }


        /// <summary>
        ///     Save a new payment or update an existin one.
        /// </summary>
        /// <param name="payment">item to save</param>
        /// <returns>whether the task has succeeded</returns>
        public bool Save(Payment payment)
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
        ///     Deletes the passed payment and removes the item from cache
        /// </summary>
        /// <param name="paymentToDelete">Payment to delete.</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Delete(Payment paymentToDelete)
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
        public void Load(Expression<Func<Payment, bool>> filter = null)
        {
            data = dataAccess.LoadList();
        }
    }
}