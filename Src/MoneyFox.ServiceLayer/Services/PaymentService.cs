using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.DataLayer.Entities;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.ServiceLayer.Services
{
    public interface IPaymentService
    {
        Task<OperationResult> SavePayment(PaymentViewModel paymentView);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ISavePaymentAction savePaymentAction;
        private readonly ISavePaymentDbAccess savePaymentDbAccess;

        public PaymentService(ISavePaymentAction savePaymentAction, ISavePaymentDbAccess savePaymentDbAccess)
        {
            this.savePaymentAction = savePaymentAction;
            this.savePaymentDbAccess = savePaymentDbAccess;
        }

        public async Task<OperationResult> SavePayment(PaymentViewModel paymentView)
        {
            var chargedAccount = await savePaymentDbAccess.GetAccount(paymentView.ChargedAccount.Id);

            Account targetAccount = null;
            if (paymentView.TargetAccount != null)
            {
                targetAccount = await savePaymentDbAccess.GetAccount(paymentView.TargetAccount.Id);
            }
            var category = await savePaymentDbAccess.GetCategory(paymentView.Category.Id);

            var payment = new Payment(paymentView.Date, paymentView.Amount, paymentView.Type, chargedAccount, targetAccount, category, paymentView.Note);
            payment.AddRecurringPayment(paymentView.RecurringPayment.Recurrence, paymentView.RecurringPayment.EndDate);

            var result = await savePaymentAction.SavePayment(payment);

            await savePaymentDbAccess.Save();

            return !result.Success 
                ? OperationResult.Failed(result.Message) 
                : OperationResult.Succeeded();
        }
    }
}
