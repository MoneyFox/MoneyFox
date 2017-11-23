using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Pocos
{
    /// <summary>
    ///     Business object for payment data.
    /// </summary>
    public class RecurringPayment
    {
        /// <summary>
        ///     Default constructor. Will Create a new RecurringPaymentEntity
        /// </summary>
        public RecurringPayment()
        {
            Data = new RecurringPaymentEntity();
        }

        /// <summary>
        ///     Set the data for this object.
        /// </summary>
        /// <param name="recurringPayment">Payment data to wrap.</param>
        public RecurringPayment(RecurringPaymentEntity recurringPayment)
        {
            Data = recurringPayment;
        }

        /// <summary>
        ///     Recurring Paymentdata
        /// </summary>
        public RecurringPaymentEntity Data { get; set; }
    }
}
