using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    public interface ISavePaymentAction
    {
        Task<OperationResult> SavePayment(Payment payment);
        Task<OperationResult> UpdatePayment(Payment newPayment);
    }

    public class SavePaymentAction: ISavePaymentAction
    {
        private readonly ISavePaymentDbAccess savePaymentDbAccess;

        public SavePaymentAction(ISavePaymentDbAccess savePaymentDbAccess)
        {
            this.savePaymentDbAccess = savePaymentDbAccess;
        }

        public async Task<OperationResult> SavePayment(Payment payment)
        {
            payment.ChargedAccount.AddPaymentAmount(payment);
            payment.TargetAccount?.AddPaymentAmount(payment);

            await savePaymentDbAccess.AddPayment(payment);
            
            return OperationResult.Succeeded();
        }

        public Task<OperationResult> UpdatePayment(Payment newPayment)
        {
            throw new System.NotImplementedException();
        }
    }
}
