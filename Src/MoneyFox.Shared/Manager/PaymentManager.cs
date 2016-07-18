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
        private readonly IDialogService dialogService;

        private readonly IUnitOfWork unitOfWork;

        public PaymentManager(IUnitOfWork unitOfWork,
            IDialogService dialogService)
        {
            this.dialogService = dialogService;

            this.unitOfWork = unitOfWork;
        }

        public bool SavePayment(Payment payment)
        {
            if (!payment.IsRecurring && payment.RecurringPaymentId != 0)
            {
                unitOfWork.RecurringPaymentRepository.Delete(payment.RecurringPayment);
                payment.RecurringPaymentId = 0;
            }
            
            bool handledRecuringPayment;
            handledRecuringPayment = payment.RecurringPayment == null || unitOfWork.RecurringPaymentRepository.Save(payment.RecurringPayment);
            if (payment.RecurringPayment != null)
            {
                payment.RecurringPaymentId = payment.RecurringPayment.Id;
            }

            var saveWasSuccessful = handledRecuringPayment && unitOfWork.PaymentRepository.Save(payment);

            return saveWasSuccessful;
        }

        public void RemoveRecurringForPayment(Payment paymentToChange)
        {
            var payments = unitOfWork.PaymentRepository.Data.Where(x => x.Id == paymentToChange.Id).ToList();

            foreach (var payment in payments)
            {
                payment.RecurringPayment = null;
                payment.IsRecurring = false;
                unitOfWork.PaymentRepository.Save(payment);
            }
        }

        public void DeleteAssociatedPaymentsFromDatabase(Account account)
        {
            if (unitOfWork.PaymentRepository.Data == null)
            {
                return;
            }

            var paymentToDelete = unitOfWork.PaymentRepository.Data
                .Where(x => x.ChargedAccountId == account.Id || x.TargetAccountId == account.Id)
                .OrderByDescending(x => x.Date)
                .ToList();

            foreach (var payment in paymentToDelete)
            {
                unitOfWork.PaymentRepository.Delete(payment);
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
            var list = unitOfWork.PaymentRepository.Data
                .Where(x => x.IsRecurring && x.RecurringPaymentId != 0)
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
            var payments = unitOfWork.PaymentRepository
                .Data
                .Where(p => !p.IsCleared)
                .Where(p => p.Date.Date <= DateTime.Now.Date);

            foreach (var payment in payments)
            {
                try
                {
                    if (payment.ChargedAccount == null)
                    {
                        payment.ChargedAccount =
                            unitOfWork.AccountRepository.Data.FirstOrDefault(x => x.Id == payment.ChargedAccountId);

                        Mvx.Trace(MvxTraceLevel.Error, "Charged account was missing while clearing payments.");
                    }

                    payment.IsCleared = true;
                    unitOfWork.PaymentRepository.Save(payment);

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
                var relatedPayment = unitOfWork.PaymentRepository
                    .Data
                    .Where(x => x.IsRecurring && x.RecurringPaymentId == recurringPayment.Id);

                foreach (var payment in relatedPayment)
                {
                    payment.IsRecurring = false;
                    payment.RecurringPaymentId = 0;
                    unitOfWork.PaymentRepository.Save(payment);
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
                    unitOfWork.AccountRepository.Data.FirstOrDefault(x => x.Id == payment.ChargedAccountId);
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
            unitOfWork.PaymentRepository.Delete(payment);

            // If this payment was the last one for the linked recurring payment
            // delete the db entry for the recurring payment.
            var succeed = true;
            if (unitOfWork.PaymentRepository.Data.All(x => x.RecurringPaymentId != payment.RecurringPaymentId))
            {
                var recurringList = unitOfWork.RecurringPaymentRepository
                    .Data
                    .Where(x => x.Id == payment.RecurringPaymentId)
                    .ToList();

                foreach (var recTrans in recurringList)
                {
                    if (!unitOfWork.RecurringPaymentRepository.Delete(recTrans))
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
            unitOfWork.AccountRepository.Save(account);
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
                trans => unitOfWork.AccountRepository.Data.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private Func<Payment, Account> GetChargedAccountFunc(Account account)
        {
            Func<Payment, Account> accountFunc =
                trans => unitOfWork.AccountRepository.Data.FirstOrDefault(x => x.Id == account.Id);
            return accountFunc;
        }
    }
}