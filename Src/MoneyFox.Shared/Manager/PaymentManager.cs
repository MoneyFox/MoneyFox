using System;
using System.Collections.Generic;
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
        private readonly IRecurringPaymentRepository recurringPaymentRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;

        public PaymentManager(IPaymentRepository paymentRepository, 
            IAccountRepository accountRepository,
            IRecurringPaymentRepository recurringPaymentRepository,
            IDialogService dialogService) {
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.dialogService = dialogService;
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
        }

        public bool SavePayment(Payment payment)
        {
            if (!payment.IsRecurring && payment.RecurringPaymentId != 0)
            {
                recurringPaymentRepository.Delete(payment.RecurringPayment);
                payment.RecurringPaymentId = 0;
            }

            bool handledRecuringPayment;
            handledRecuringPayment = payment.RecurringPayment == null || recurringPaymentRepository.Save(payment.RecurringPayment);

            if (payment.RecurringPayment != null)
            {
                payment.RecurringPaymentId = payment.RecurringPayment.Id;
            }

            var saveWasSuccessful = handledRecuringPayment && paymentRepository.Save(payment);

            return saveWasSuccessful;
        }

        public void RemoveRecurringForPayment(Payment paymentToChange)
        {
            var payments = paymentRepository.GetList(x => x.Id == paymentToChange.Id).ToList();

            foreach (var payment in payments)
            {
                payment.RecurringPayment = null;
                payment.IsRecurring = false;
                paymentRepository.Save(payment);
            }
        }

        public void DeleteAssociatedPaymentsFromDatabase(Account account)
        {
            var paymentToDelete = paymentRepository
                .GetList(x => x.ChargedAccountId == account.Id || x.TargetAccountId == account.Id)
                .OrderByDescending(x => x.Date)
                .ToList();

            foreach (var payment in paymentToDelete)
            {
                paymentRepository.Delete(payment);
            }
        }

        public async Task<bool> CheckRecurrenceOfPayment(Payment payment)
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

        /// <summary>
        ///     returns a list with payments who recure in a given timeframe
        /// </summary>
        /// <returns>list of recurring payments</returns>
        public IEnumerable<Payment> LoadRecurringPaymentList(Func<Payment, bool> filter = null)
        {
            var list = paymentRepository
                .GetList(x => x.IsRecurring && x.RecurringPaymentId != 0)
                .Where(x => (x.RecurringPayment.IsEndless ||
                             x.RecurringPayment.EndDate >= DateTime.Now.Date)
                            && (filter == null || filter.Invoke(x)))
                .ToList();

            return list
                .Select(x => x.RecurringPaymentId)
                .Distinct()
                .Select(id => list.Where(x => x.RecurringPaymentId == id)
                    .OrderByDescending(x => x.Date)
                    .Last())
                .ToList();
        }

        public void ClearPayments()
        {
            var payments = paymentRepository
                .GetList(p => !p.IsCleared && p.Date.Date <= DateTime.Now.Date);

            foreach (var payment in payments)
            {
                try
                {
                    if (payment.ChargedAccount == null)
                    {
                        payment.ChargedAccount =
                            accountRepository.GetList(x => x.Id == payment.ChargedAccountId).FirstOrDefault();

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
                    .GetList(x => x.IsRecurring && x.RecurringPaymentId == recurringPayment.Id);

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
                payment.Type == (int) PaymentType.Income
                    ? x
                    : -x;

            if (payment.ChargedAccount == null && payment.ChargedAccountId != 0)
            {
                payment.ChargedAccount =
                    accountRepository.GetList(x => x.Id == payment.ChargedAccountId).FirstOrDefault();
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
                payment.Type == (int) PaymentType.Income
                    ? -x
                    : x;

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(account));
            return true;
        }

        public bool DeletePayment(Payment payment)
        {
            paymentRepository.Delete(payment);

            // If this payment was the last one for the linked recurring payment
            // delete the db entry for the recurring payment.
            var succeed = true;
            if (paymentRepository.GetList().All(x => x.RecurringPaymentId != payment.RecurringPaymentId))
            {
                var recurringList = recurringPaymentRepository
                    .GetList(x => x.Id == payment.RecurringPaymentId)
                    .ToList();

                foreach (var recTrans in recurringList)
                {
                    if (!recurringPaymentRepository.Delete(recTrans))
                    {
                        succeed = false;
                    }
                }
            }
            return succeed;
        }

        private void PrehandleRemoveIfTransfer(Payment payment)
        {
            if (payment.Type == (int) PaymentType.Transfer)
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
            if (payment.Type == (int) PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<Payment, Account> GetTargetAccountFunc()
        {
            Func<Payment, Account> targetAccountFunc =
                trans => accountRepository.GetList(x => x.Id == trans.TargetAccountId).FirstOrDefault();
            return targetAccountFunc;
        }

        private Func<Payment, Account> GetChargedAccountFunc(Account account)
        {
            Func<Payment, Account> accountFunc =
                trans => accountRepository.GetList(x => x.Id == account.Id).FirstOrDefault();
            return accountFunc;
        }
    }
}