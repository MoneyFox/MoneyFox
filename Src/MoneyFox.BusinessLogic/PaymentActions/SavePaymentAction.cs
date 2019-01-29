using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    /// <summary>
    ///     Contains actions to save new an updated payments.
    /// </summary>
    public interface ISavePaymentAction
    {
        /// <summary>
        ///     Create a new payment on the database.
        /// </summary>
        /// <param name="payment">new Payment</param>
        /// <returns>Result</returns>
        Task<OperationResult> AddPayment(Payment payment);

        /// <summary>
        ///     Update an existing payment.
        /// </summary>
        /// <param name="id">Id of the payment to update.</param>
        /// <param name="newPayment">new payments.</param>
        /// <returns>Result.</returns>
        Task<OperationResult> UpdatePayment(int id, Payment newPayment);
    }

    public class SavePaymentAction : ISavePaymentAction
    {
        private readonly ISavePaymentDbAccess savePaymentDbAccess;

        public SavePaymentAction(ISavePaymentDbAccess savePaymentDbAccess)
        {
            this.savePaymentDbAccess = savePaymentDbAccess;
        }

        public async Task<OperationResult> AddPayment(Payment payment)
        {
            payment.ChargedAccount.AddPaymentAmount(payment);
            payment.TargetAccount?.AddPaymentAmount(payment);

            await savePaymentDbAccess.AddPayment(payment)
                                     .ConfigureAwait(false);

            return OperationResult.Succeeded();
        }

        public async Task<OperationResult> UpdatePayment(int id, Payment newPayment)
        {
            var paymentFromDatabase = await savePaymentDbAccess.GetPaymentById(id)
                                                      .ConfigureAwait(false);

            paymentFromDatabase.ChargedAccount.RemovePaymentAmount(paymentFromDatabase);
            paymentFromDatabase.TargetAccount?.RemovePaymentAmount(paymentFromDatabase);

            newPayment.ChargedAccount.AddPaymentAmount(newPayment);
            newPayment.TargetAccount?.AddPaymentAmount(newPayment);

            paymentFromDatabase.UpdatePayment(newPayment.Date,
                                     newPayment.Amount,
                                     newPayment.Type,
                                     newPayment.ChargedAccount,
                                     newPayment.TargetAccount,
                                     newPayment.Category,
                                     newPayment.Note);

            return OperationResult.Succeeded();
        }
    }
}