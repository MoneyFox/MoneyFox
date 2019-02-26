using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Interfaces;
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
        private readonly IModifyPaymentAction modifyPaymentAction;
        private readonly IDialogService dialogService;


        public PaymentService(EfCoreContext context, IModifyPaymentAction modifyPaymentAction, IDialogService dialogService)
        {
            this.modifyPaymentAction = modifyPaymentAction;
            this.dialogService = dialogService;
            this.context = context;
        }

        public async Task<OperationResult> SavePayment(PaymentViewModel paymentViewModel)
        {
            var payment = await CreatePaymentFromViewModel(paymentViewModel).ConfigureAwait(true);

            var result = await modifyPaymentAction.AddPayment(payment)
                .ConfigureAwait(true);

            await context.SaveChangesAsync()
                .ConfigureAwait(true);

            return !result.Success
                ? OperationResult.Failed(result.Message)
                : OperationResult.Succeeded();
        }

        public async Task<OperationResult> UpdatePayment(PaymentViewModel newPaymentViewModel)
        {
            await UpdatePaymentFromViewModel(newPaymentViewModel).ConfigureAwait(true);

            await context.SaveChangesAsync()
                .ConfigureAwait(true);

            return OperationResult.Succeeded();
        }

        public async Task<OperationResult> DeletePayment(PaymentViewModel paymentViewModel)
        {
            if (!await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)
                .ConfigureAwait(true)) return OperationResult.Succeeded();

            if (paymentViewModel.IsRecurring
                && await dialogService
                    .ShowConfirmMessage(Strings.DeleteRecurringPaymentTitle, Strings.DeleteRecurringPaymentMessage)
                    .ConfigureAwait(true))
            {
                await modifyPaymentAction.DeleteRecurringPayment(paymentViewModel.RecurringPayment.Id)
                    .ConfigureAwait(true);
            }
            
            var result = await modifyPaymentAction.DeletePayment(paymentViewModel.Id)
                .ConfigureAwait(true);

            await context.SaveChangesAsync()
                .ConfigureAwait(true);

            return !result.Success
                ? OperationResult.Failed(result.Message)
                : OperationResult.Succeeded();
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
                .FirstAsync(x => x.Id == paymentViewModel.Id)
                .ConfigureAwait(true);

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
                    .ConfigureAwait(true))
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
                    .FindAsync(paymentViewModel.Category.Id)
                    .ConfigureAwait(true);
            return category;
        }

        private async Task<Account> GetChargedAccount(PaymentViewModel paymentViewModel)
        {
            var chargedAccount = await context.Accounts
                .FindAsync(paymentViewModel.ChargedAccount.Id)
                .ConfigureAwait(true);
            return chargedAccount;
        }

        private async Task<Account> GetTargetAccount(PaymentViewModel paymentViewModel)
        {
            Account targetAccount = null;
            if (paymentViewModel.TargetAccount != null)
                targetAccount = await context.Accounts
                    .FindAsync(paymentViewModel.TargetAccount.Id)
                    .ConfigureAwait(true);
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