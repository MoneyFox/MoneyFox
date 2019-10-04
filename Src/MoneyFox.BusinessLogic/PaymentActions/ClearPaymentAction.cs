using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    /// <summary>
    ///     Manager to handle payment clearing.
    /// </summary>
    public interface IClearPaymentAction
    {
        /// <summary>
        ///     Clears all payments up for clearing.
        ///     After this save change has to be called on the context.
        /// </summary>
        Task ClearPayments();
    }

    /// <inheritdoc />
    public class ClearPaymentAction : IClearPaymentAction
    {
        private readonly IClearPaymentDbAccess clearPaymentDbAccess;

        public ClearPaymentAction(IClearPaymentDbAccess clearPaymentDbAccess)
        {
            this.clearPaymentDbAccess = clearPaymentDbAccess;
        }

        /// <inheritdoc />
        public async Task ClearPayments()
        {
            var payments = await clearPaymentDbAccess.GetUnclearedPayments()
                                                     ;

            foreach (var payment in payments)
            {
                payment.ClearPayment();
            }
        }
    }
}
