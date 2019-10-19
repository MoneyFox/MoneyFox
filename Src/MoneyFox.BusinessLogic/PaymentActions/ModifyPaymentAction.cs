using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.Domain.Entities;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    /// <summary>
    ///     Contains actions to save new an updated payments.
    /// </summary>
    public interface IModifyPaymentAction
    {
        /// <summary>
        ///     Delete an existing Payment.
        /// </summary>
        /// <param name="id">Id of the payment to delete.</param>
        /// <returns>Result.</returns>
        Task DeletePayment(int id);

        /// <summary>
        ///     Delete an existing Recurring Payment.
        /// </summary>
        /// <param name="id">Id of the payment to delete.</param>
        /// <returns>Result.</returns>
        Task DeleteRecurringPayment(int id);
    }

    public class ModifyPaymentAction : IModifyPaymentAction
    {
        private readonly ISavePaymentDbAccess savePaymentDbAccess;

        public ModifyPaymentAction(ISavePaymentDbAccess savePaymentDbAccess)
        {
            this.savePaymentDbAccess = savePaymentDbAccess;
        }

        public async Task DeletePayment(int id)
        {
            Payment payment = await savePaymentDbAccess.GetPaymentById(id);

            payment.ChargedAccount.RemovePaymentAmount(payment);
            payment.TargetAccount?.RemovePaymentAmount(payment);

            savePaymentDbAccess.DeletePayment(payment);
        }

        public async Task DeleteRecurringPayment(int id)
        {
            List<Payment> payments = await savePaymentDbAccess.GetPaymentsForRecurring(id);

            payments.ForEach(x => x.RemoveRecurringPayment());

            await savePaymentDbAccess.DeleteRecurringPayment(id);
        }
    }
}
