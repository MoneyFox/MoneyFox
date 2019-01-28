using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.DataLayer;
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
        private readonly EfCoreContext context;
        private readonly ISavePaymentAction savePaymentAction;


        public PaymentService(EfCoreContext context, ISavePaymentAction savePaymentAction)
        {
            this.savePaymentAction = savePaymentAction;
            this.context = context;
        }

        public async Task<OperationResult> SavePayment(PaymentViewModel paymentView)
        {
            Account chargedAccount = await context.Accounts.FindAsync(paymentView.ChargedAccount.Id);

            Account targetAccount = null;
            if (paymentView.TargetAccount != null)
            {
                targetAccount = await context.Accounts.FindAsync(paymentView.TargetAccount.Id);
            }
            Category category;
            if (paymentView.Category != null)
            {
                category = await context.Categories.FindAsync(paymentView.Category.Id);
            }

            var payment = new Payment(paymentView.Date, paymentView.Amount, paymentView.Type, chargedAccount, targetAccount, category, paymentView.Note);
            payment.AddRecurringPayment(paymentView.RecurringPayment.Recurrence, paymentView.RecurringPayment.EndDate);

            var result = await savePaymentAction.SavePayment(payment);

            await context.SaveChangesAsync();

            return !result.Success 
                ? OperationResult.Failed(result.Message) 
                : OperationResult.Succeeded();
        }
    }
}
