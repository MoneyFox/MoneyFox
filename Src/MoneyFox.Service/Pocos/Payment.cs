using MoneyFox.DataAccess.Entities;

namespace MoneyFox.Service.Pocos
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
    }
}
