using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Interfaces;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Services
{
    /// <summary>
    ///     Service to coordinate with several operations for payments
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        ///     Updates a payment.
        /// </summary>
        /// <param name="newPaymentViewModel">View model which contains the view data.</param>
        /// <returns>Result</returns>
        Task UpdatePayment(PaymentViewModel newPaymentViewModel);
    }

    /// <inheritdoc />
    public class PaymentService : IPaymentService
    {
        private readonly IEfCoreContext context;
        private readonly IModifyPaymentAction modifyPaymentAction;
        private readonly IDialogService dialogService;

        public PaymentService(IEfCoreContext context, IModifyPaymentAction modifyPaymentAction, IDialogService dialogService)
        {
            this.modifyPaymentAction = modifyPaymentAction;
            this.dialogService = dialogService;
            this.context = context;
        }

        /// <inheritdoc />
        public async Task UpdatePayment(PaymentViewModel newPaymentViewModel)
        {
            await UpdatePaymentFromViewModel(newPaymentViewModel);

            await context.SaveChangesAsync();
        }

        private async Task UpdatePaymentFromViewModel(PaymentViewModel paymentViewModel)
        {
            Payment payment = await context.Payments
                                           .Include(x => x.RecurringPayment)
                                           .FirstAsync(x => x.Id == paymentViewModel.Id);

            Account chargedAccount = await GetChargedAccount(paymentViewModel);
            Account targetAccount = await GetTargetAccount(paymentViewModel);
            Category category = await GetCategory(paymentViewModel);

            payment.UpdatePayment(paymentViewModel.Date,
                                  paymentViewModel.Amount,
                                  paymentViewModel.Type,
                                  chargedAccount,
                                  targetAccount,
                                  category,
                                  paymentViewModel.Note);

            if (paymentViewModel.IsRecurring
                && payment.IsRecurring
                && await dialogService
                    .ShowConfirmMessageAsync(Strings.ModifyRecurrenceTitle, Strings.ModifyRecurrenceMessage)
            )
            {
                payment.RecurringPayment.UpdateRecurringPayment(payment.Amount,
                                                                paymentViewModel.RecurringPayment.Recurrence,
                                                                payment.ChargedAccount,
                                                                payment.Note,
                                                                paymentViewModel.RecurringPayment.IsEndless
                                                                    ? null
                                                                    : paymentViewModel.RecurringPayment.EndDate,
                                                                payment.TargetAccount);
            }
        }

        private async Task<Category> GetCategory(PaymentViewModel paymentViewModel)
        {
            Category category = null;
            if (paymentViewModel.Category != null)
            {
                category = await context.Categories
                                        .FindAsync(paymentViewModel.Category.Id);
            }

            return category;
        }

        private async Task<Account> GetChargedAccount(PaymentViewModel paymentViewModel)
        {
            Account chargedAccount = await context.Accounts
                                                  .FindAsync(paymentViewModel.ChargedAccount.Id);

            return chargedAccount;
        }

        private async Task<Account> GetTargetAccount(PaymentViewModel paymentViewModel)
        {
            Account targetAccount = null;
            if (paymentViewModel.TargetAccount != null)
            {
                targetAccount = await context.Accounts
                                             .FindAsync(paymentViewModel.TargetAccount.Id);
            }

            return targetAccount;
        }
    }
}
