using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Payment> data;

        public PaymentRepository(IDataAccess<Payment> dataAccess)
        {
            this.dataAccess = dataAccess;

            Data = new ObservableCollection<Payment>();
            Load();
        }


        /// <summary>
        ///     Cached accountToDelete data
        /// </summary>
        public ObservableCollection<Payment> Data
        {
            get { return data; }
            set
            {
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public Payment FindById(int id) => data.FirstOrDefault(p => p.Id == id);

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
            data.Remove(paymentToDelete);
            return dataAccess.DeleteItem(paymentToDelete);
        }

        /// <summary>
        ///     Loads all payments from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Payment, bool>> filter = null)
        {
            Data.Clear();

            foreach (var payment in dataAccess.LoadList(filter))
            {
                Data.Add(payment);
            }
        }
    }
}