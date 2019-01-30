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
        Task<OperationResult> SavePayment(PaymentViewModel paymentViewModel);

        Task<OperationResult> UpdatePayment(PaymentViewModel newPaymentViewModel);

        Task<OperationResult> DeletePayment(PaymentViewModel paymentViewModel);
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

        public async Task<OperationResult> SavePayment(PaymentViewModel paymentViewModel)
        {
            var payment = await CreatePaymentFromViewModel(paymentViewModel).ConfigureAwait(false);

            var result = await savePaymentAction.AddPayment(payment)
                .ConfigureAwait(false);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);

            return !result.Success
                ? OperationResult.Failed(result.Message)
                : OperationResult.Succeeded();
        }

        public async Task<OperationResult> UpdatePayment(PaymentViewModel newPaymentViewModel)
        {
            var payment = await CreatePaymentFromViewModel(newPaymentViewModel).ConfigureAwait(false);

            var result = await savePaymentAction.UpdatePayment(newPaymentViewModel.Id, payment)
                .ConfigureAwait(false);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);

            return !result.Success
                ? OperationResult.Failed(result.Message)
                : OperationResult.Succeeded();
        }

        public async Task<OperationResult> DeletePayment(PaymentViewModel paymentViewModel)
        {
            var result = await savePaymentAction.DeletePayment(paymentViewModel.Id)
                .ConfigureAwait(false);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);

            return !result.Success
                ? OperationResult.Failed(result.Message)
                : OperationResult.Succeeded();
        }

        private async Task<Payment> CreatePaymentFromViewModel(PaymentViewModel paymentViewModel)
        {
            var chargedAccount = await context.Accounts
                .FindAsync(paymentViewModel.ChargedAccount.Id)
                .ConfigureAwait(false);

            Account targetAccount = null;
            if (paymentViewModel.TargetAccount != null)
                targetAccount = await context.Accounts
                    .FindAsync(paymentViewModel.TargetAccount.Id)
                    .ConfigureAwait(false);

            Category category = null;
            if (paymentViewModel.Category != null)
                category = await context.Categories
                    .FindAsync(paymentViewModel.Category.Id)
                    .ConfigureAwait(false);

            var payment = new Payment(paymentViewModel.Date, paymentViewModel.Amount, paymentViewModel.Type,
                chargedAccount,
                targetAccount, category, paymentViewModel.Note);

            if (paymentViewModel.IsRecurring)
                payment.AddRecurringPayment(paymentViewModel.RecurringPayment.Recurrence,
                    paymentViewModel.RecurringPayment.EndDate);

            return payment;
        }
    }
}