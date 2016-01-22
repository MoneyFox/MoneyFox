using System;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using Xamarin;

namespace MoneyManager.Core.Manager
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentRepository paymentRepository;

        /// <summary>
        ///     Creates an PaymentManager object.
        /// </summary>
        /// <param name="paymentRepository">Instance of <see cref="IPaymentRepository" /></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}" /></param>
        /// <param name="dialogService">Instance of <see cref="IDialogService" /></param>
        public PaymentManager(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository, IDialogService dialogService)
        {
            this.accountRepository = accountRepository;
            this.dialogService = dialogService;
            this.paymentRepository = paymentRepository;
        }

        public void DeleteAssociatedPaymentsFromDatabase(Account account)
        {
            if (paymentRepository.Data == null)
            {
                return;
            }

            var paymentToDelete = paymentRepository.GetRelatedPayments(account);

            foreach (var payment in paymentToDelete)
            {
                paymentRepository.Delete(payment);
            }
        }

        public async Task<bool> CheckForRecurringPayment(Payment payment)
        {
            if (!payment.IsRecurring)
            {
                return false;
            }

            return
                await
                    dialogService.ShowConfirmMessage(Strings.ChangeSubsequentPaymentTitle,
                        Strings.ChangeSubsequentPaymentMessage,
                        Strings.RecurringLabel, Strings.JustThisLabel);
        }

        public void ClearPayments()
        {
            var payments = paymentRepository.GetUnclearedPayments();
            foreach (var payment in payments)
            {
                try
                {
                    if (payment.ChargedAccount == null)
                    {
                        payment.ChargedAccount =
                            accountRepository.Data.FirstOrDefault(x => x.Id == payment.ChargedAccountId);

                        Insights.Report(new AccountMissingException("Charged account was missing while clearing transactions"), Insights.Severity.Error);
                    }

                    payment.IsCleared = true;
                    paymentRepository.Save(payment);

                    accountRepository.AddTransactionAmount(payment);
                }
                catch (Exception ex)
                {
                    Insights.Report(ex, Insights.Severity.Error);
                }
            }
        }

        public void RemoveRecurringForPayments(RecurringPayment recurringPayment)
        {
            try
            {
                var relatedPayment = paymentRepository
                    .Data
                    .Where(x => x.IsRecurring && x.ReccuringTransactionId == recurringPayment.Id);

                foreach (var payment in relatedPayment)
                {
                    payment.IsRecurring = false;
                    payment.ReccuringTransactionId = 0;
                    paymentRepository.Save(payment);
                }
            }
            catch (Exception ex)
            {
                Insights.Report(ex, Insights.Severity.Error);
            }
        }
    }
}