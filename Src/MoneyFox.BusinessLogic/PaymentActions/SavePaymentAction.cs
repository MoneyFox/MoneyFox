using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    public interface ISavePaymentAction
    {
        Task<OperationResult> SavePayment(Payment payment);
    }

    public class SavePaymentAction: ISavePaymentAction
    {
        private ISavePaymentDbAccess savePaymentDbAccess;

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
    }
}
