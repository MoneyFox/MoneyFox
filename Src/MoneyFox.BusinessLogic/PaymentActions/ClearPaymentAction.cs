using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.Domain.Entities;

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
        Task ClearPaymentsAsync();
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
        public async Task ClearPaymentsAsync()
        {
            List<Payment> payments = await clearPaymentDbAccess.GetUnclearedPaymentsAsync()
                ;

            foreach (Payment payment in payments)
            {
                payment.ClearPayment();
            }
        }
    }
}
