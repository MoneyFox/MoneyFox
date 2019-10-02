using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Interfaces;
using MoneyFox.Presentation.ViewModels;
using NLog;

namespace MoneyFox.Presentation.Services
{
    /// <summary>
    ///     Service to coordinate with several operations for payments
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        ///     Save a new payment
        /// </summary>
        /// <param name="paymentViewModel">View model which contains the view data.</param>
        /// <returns>Result</returns>
        Task SavePayment(PaymentViewModel paymentViewModel);

        /// <summary>
        ///     Updates a payment.
        /// </summary>
        /// <param name="newPaymentViewModel">View model which contains the view data.</param>
        /// <returns>Result</returns>
        Task UpdatePayment(PaymentViewModel newPaymentViewModel);

        /// <summary>
        ///     Deletes a existing payment
        /// </summary>
        /// <param name="paymentViewModel">View model which contains the view data.</param>
        /// <returns>Result</returns>
        Task DeletePayment(PaymentViewModel paymentViewModel);
    }

    /// <inheritdoc />
    public class PaymentService : IPaymentService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

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
        public async Task SavePayment(PaymentViewModel paymentViewModel)
        {
            var payment = await CreatePaymentFromViewModel(paymentViewModel);
            await context.AddAsync(payment);

            var count = await context.SaveChangesAsync();
            logger.Info("{count} entities saved.", count);
        }

        /// <inheritdoc />
        public async Task UpdatePayment(PaymentViewModel newPaymentViewModel)
        {
            await UpdatePaymentFromViewModel(newPaymentViewModel);

            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeletePayment(PaymentViewModel paymentViewModel)
        {
            if (!await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) return;

            if (paymentViewModel.IsRecurring
                && await dialogService.ShowConfirmMessage(Strings.DeleteRecurringPaymentTitle, Strings.DeleteRecurringPaymentMessage))
            {
                await modifyPaymentAction.DeleteRecurringPayment(paymentViewModel.RecurringPayment.Id);
            }
            
            await modifyPaymentAction.DeletePayment(paymentViewModel.Id);
            await context.SaveChangesAsync();
        }

        private async Task<Payment> CreatePaymentFromViewModel(PaymentViewModel paymentViewModel)
        {
            var chargedAccount = await GetChargedAccount(paymentViewModel);
            var targetAccount = await GetTargetAccount(paymentViewModel);
            var category = await GetCategory(paymentViewModel);

            var payment = new Payment(paymentViewModel.Date, paymentViewModel.Amount, paymentViewModel.Type,
                chargedAccount,
                targetAccount, category, paymentViewModel.Note);
            try
            {
                AddRecurringPayment(paymentViewModel, payment);
                return payment;
            }
            catch (Exception)
            {
                payment.ChargedAccount.RemovePaymentAmount(payment);
                payment.TargetAccount?.RemovePaymentAmount(payment);
                throw;
            }
        }

        private async Task UpdatePaymentFromViewModel(PaymentViewModel paymentViewModel)
        {
            var payment = await context.Payments
                .Include(x => x.RecurringPayment)
                .FirstAsync(x => x.Id == paymentViewModel.Id);

            var chargedAccount = await GetChargedAccount(paymentViewModel);
            var targetAccount = await GetTargetAccount(paymentViewModel);
            var category = await GetCategory(paymentViewModel);

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
                    .ShowConfirmMessage(Strings.ModifyRecurrenceTitle, Strings.ModifyRecurrenceMessage)
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
                category = await context.Categories
                    .FindAsync(paymentViewModel.Category.Id);
            return category;
        }

        private async Task<Account> GetChargedAccount(PaymentViewModel paymentViewModel)
        {
            var chargedAccount = await context.Accounts
                .FindAsync(paymentViewModel.ChargedAccount.Id);
            return chargedAccount;
        }

        private async Task<Account> GetTargetAccount(PaymentViewModel paymentViewModel)
        {
            Account targetAccount = null;
            if (paymentViewModel.TargetAccount != null)
                targetAccount = await context.Accounts
                    .FindAsync(paymentViewModel.TargetAccount.Id);
            return targetAccount;
        }

        private static void AddRecurringPayment(PaymentViewModel paymentViewModel, Payment payment)
        {
            if (paymentViewModel.IsRecurring)
                payment.AddRecurringPayment(paymentViewModel.RecurringPayment.Recurrence,
                    paymentViewModel.RecurringPayment.IsEndless
                        ? null
                        : paymentViewModel.RecurringPayment.EndDate);
        }
    }
}
