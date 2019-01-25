using MoneyFox.Foundation;

namespace MoneyFox.ServiceLayer.Parameters
{
    /// <summary>
    ///     Parameter object for the ModifyPaymentView.
    /// </summary>
    public class ModifyPaymentParameter
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="paymentId">Payment Id to edit</param>
        public ModifyPaymentParameter(int paymentId = 0)
        {
            PaymentId = paymentId;
        }

        /// <summary>
        ///     constuctor
        /// </summary>
        /// <param name="paymentType">Type for a new payment.</param>
        /// <param name="paymentId">Payment Id to edit</param>
        public ModifyPaymentParameter(PaymentType paymentType, int paymentId = 0) : this(paymentId)
        {
            PaymentType = paymentType;
        }

        /// <summary>
        ///     PaymentId who shall be edited.
        ///     If this is 0, a new account shall be created.
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        ///     Type of the new payment.
        /// </summary>
        public PaymentType PaymentType { get; set; }
    }
}
