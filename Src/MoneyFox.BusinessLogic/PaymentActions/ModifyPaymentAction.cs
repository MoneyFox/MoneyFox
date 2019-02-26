using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    /// <summary>
    ///     Contains actions to save new an updated payments.
    /// </summary>
    public interface IModifyPaymentAction
    {
        /// <summary>
        ///     Create a new payment on the database.
        /// </summary>
        /// <param name="payment">new Payment</param>
        /// <returns>Result</returns>
        Task<OperationResult> AddPayment(Payment payment);

        /// <summary>
        ///     Delete an existing Payment.
        /// </summary>
        /// <param name="id">Id of the payment to delete.</param>
        /// <returns>Result.</returns>
        Task<OperationResult> DeletePayment(int id);

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

        public async Task<OperationResult> AddPayment(Payment payment)
        {
            await savePaymentDbAccess.AddPayment(payment)
                                     .ConfigureAwait(true);

            return OperationResult.Succeeded();
        }

        public async Task<OperationResult> DeletePayment(int id)
        {
            var payment = await savePaymentDbAccess.GetPaymentById(id)
                                                   .ConfigureAwait(true);
            
            payment.ChargedAccount.RemovePaymentAmount(payment);
            payment.TargetAccount?.RemovePaymentAmount(payment);

            savePaymentDbAccess.DeletePayment(payment);

            return OperationResult.Succeeded();
        }

        public async Task DeleteRecurringPayment(int id)
        {
            var payments = await savePaymentDbAccess.GetPaymentsForRecurring(id).ConfigureAwait(true);

            payments.ForEach(x => x.RemoveRecurringPayment());

            await savePaymentDbAccess.DeleteRecurringPayment(id)
                .ConfigureAwait(true);
        }
    }
}