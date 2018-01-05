using System;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Pocos
{
    /// <summary>
    ///     Business object for payment data.
    /// </summary>
    public class Payment
    {
        /// <summary>
        ///     Default constructor. Will Create a new PaymentEntity
        /// </summary>
        public Payment()
        {
            Data = new PaymentEntity();
        }

        /// <summary>
        ///     Set the data for this object.
        /// </summary>
        /// <param name="payment">Payment data to wrap.</param>
        public Payment(PaymentEntity payment)
        {
            Data = payment;
        }

        /// <summary>
        ///     Paymentdata
        /// </summary>
        public PaymentEntity Data { get; set; }

        /// <summary>
        ///     This method checks if the payment is up for clearing and sets the
        ///     flags accordingly.
        /// </summary>
        public void ClearPayment()
        {
            if (Data.IsCleared) return;
            Data.IsCleared = Data.Date.Date <= DateTime.Now.Date;
        }
    }
}
