using System;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Shared.Manager
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
            IAccountRepository accountRepository, 
            IDialogService dialogService)
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

            var paymentToDelete = paymentRepository.GetRelatedPayments(account.Id);

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

                        Mvx.Trace(MvxTraceLevel.Error, "Charged account was missing while clearing payments.");
                    }

                    payment.IsCleared = true;
                    paymentRepository.Save(payment);

                    AddPaymentAmount(payment);
                }
                catch (Exception ex)
                {
                    Mvx.Trace(MvxTraceLevel.Error, ex.Message);
                }
            }
        }

        public void RemoveRecurringForPayments(RecurringPayment recurringPayment)
        {
            try
            {
                var relatedPayment = paymentRepository
                    .Data
                    .Where(x => x.IsRecurring && x.RecurringPaymentId == recurringPayment.Id);

                foreach (var payment in relatedPayment)
                {
                    payment.IsRecurring = false;
                    payment.RecurringPaymentId = 0;
                    paymentRepository.Save(payment);
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
        }


        /// <summary>
        ///     Adds the payment amount from the selected account
        /// </summary>
        /// <param name="payment">Payment to add the account from.</param>
        public bool AddPaymentAmount(Payment payment)
        {
            if (!payment.IsCleared)
            {
                return false;
            }

            PrehandleAddIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == (int)PaymentType.Income
                    ? x
                    : -x;

            if (payment.ChargedAccount == null && payment.ChargedAccountId != 0)
            {
                payment.ChargedAccount = accountRepository.Data.FirstOrDefault(x => x.Id == payment.ChargedAccountId);
            }

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(payment.ChargedAccount));
            return true;
        }

        /// <summary>
        ///     Removes the payment Amount from the charged account of this payment
        /// </summary>
        /// <param name="payment">Payment to remove the account from.</param>
        public bool RemovePaymentAmount(Payment payment)
        {
            var succeded = RemovePaymentAmount(payment, payment.ChargedAccount);
            return succeded;
        }

        /// <summary>
        ///     Removes the payment Amount from the selected account
        /// </summary>
        /// <param name="payment">Payment to remove.</param>
        /// <param name="account">Account to remove the amount from.</param>
        public bool RemovePaymentAmount(Payment payment, Account account)
        {
            if (!payment.IsCleared)
            {
                return false;
            }

            PrehandleRemoveIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == (int)PaymentType.Income
                    ? -x
                    : x;

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(account));
            return true;
        }

        private void PrehandleRemoveIfTransfer(Payment payment)
        {
            if (payment.Type == (int)PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandlePaymentAmount(Payment payment,
            Func<double, double> amountFunc,
            Func<Payment, Account> getAccountFunc)
        {
            var account = getAccountFunc(payment);
            if (account == null)
            {
                return;
            }

            account.CurrentBalance += amountFunc(payment.Amount);
            accountRepository.Save(account);
        }

        private void PrehandleAddIfTransfer(Payment payment)
        {
            if (payment.Type == (int)PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<Payment, Account> GetTargetAccountFunc()
        {
            Func<Payment, Account> targetAccountFunc =
                trans => accountRepository.Data.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private Func<Payment, Account> GetChargedAccountFunc(Account account)
        {
            Func<Payment, Account> accountFunc =
                trans => accountRepository.Data.FirstOrDefault(x => x.Id == account.Id);
            return accountFunc;
        }
    }
}