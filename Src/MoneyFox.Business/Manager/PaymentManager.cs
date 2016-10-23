using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Business.Manager
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentRepository paymentRepository;
        private readonly IRecurringPaymentRepository recurringPaymentRepository;

        public PaymentManager(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IRecurringPaymentRepository recurringPaymentRepository,
            IDialogService dialogService)
        {
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.dialogService = dialogService;
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
        }

        public bool SavePayment(PaymentViewModel payment)
        {
            if (!payment.IsRecurring && (payment.RecurringPaymentId != 0))
            {
                recurringPaymentRepository.Delete(payment.RecurringPayment);
                payment.RecurringPaymentId = 0;
            }

            bool handledRecuringPayment;
            handledRecuringPayment = (payment.RecurringPayment == null) ||
                                     recurringPaymentRepository.Save(payment.RecurringPayment);

            if (payment.RecurringPayment != null)
            {
                payment.RecurringPaymentId = payment.RecurringPayment.Id;
            }

            var saveWasSuccessful = handledRecuringPayment && paymentRepository.Save(payment);

            return saveWasSuccessful;
        }

        public void RemoveRecurringForPayment(PaymentViewModel paymentToChange)
        {
            var payments = paymentRepository.GetList(x => x.Id == paymentToChange.Id).ToList();

            foreach (var payment in payments)
            {
                payment.RecurringPayment = null;
                payment.IsRecurring = false;
                paymentRepository.Save(payment);
            }
        }

        public void DeleteAssociatedPaymentsFromDatabase(AccountViewModel accountViewModel)
        {
            var paymentToDelete = paymentRepository
                .GetList(x => (x.ChargedAccountId == accountViewModel.Id) || (x.TargetAccountId == accountViewModel.Id))
                .OrderByDescending(x => x.Date)
                .ToList();

            foreach (var payment in paymentToDelete)
            {
                paymentRepository.Delete(payment);
            }
        }

        public async Task<bool> CheckRecurrenceOfPayment(PaymentViewModel payment)
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
        public IEnumerable<PaymentViewModel> LoadRecurringPaymentList(Func<PaymentViewModel, bool> filter = null)
        {
            var list = paymentRepository
                .GetList(x => x.IsRecurring && (x.RecurringPaymentId != 0))
                .Where(x => (x.RecurringPayment.IsEndless ||
                             (x.RecurringPayment.EndDate >= DateTime.Now.Date))
                            && ((filter == null) || filter.Invoke(x)))
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
                .GetList(p => !p.IsCleared && (p.Date.Date <= DateTime.Now.Date));

            foreach (var payment in payments)
            {
                try
                {
                    if (payment.ChargedAccount == null)
                    {
                        payment.ChargedAccount =
                            accountRepository.GetList(x => x.Id == payment.ChargedAccountId).FirstOrDefault();

                        Mvx.Trace(MvxTraceLevel.Error, "Charged AccountViewModel was missing while clearing payments.");
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

        public void RemoveRecurringForPayments(RecurringPaymentViewModel recurringPayment)
        {
            try
            {
                var relatedPayment = paymentRepository
                    .GetList(x => x.IsRecurring && (x.RecurringPaymentId == recurringPayment.Id));

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
        ///     Adds the PaymentViewModel amount from the selected AccountViewModel
        /// </summary>
        /// <param name="payment">PaymentViewModel to add the AccountViewModel from.</param>
        public bool AddPaymentAmount(PaymentViewModel payment)
        {
            if (!payment.IsCleared)
            {
                return false;
            }

            PrehandleAddIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == PaymentType.Income
                    ? x
                    : -x;

            if ((payment.ChargedAccount == null) && (payment.ChargedAccountId != 0))
            {
                payment.ChargedAccount =
                    accountRepository.GetList(x => x.Id == payment.ChargedAccountId).FirstOrDefault();
            }

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(payment.ChargedAccount));
            return true;
        }

        /// <summary>
        ///     Removes the PaymentViewModel Amount from the charged AccountViewModel of this PaymentViewModel
        /// </summary>
        /// <param name="payment">PaymentViewModel to remove the AccountViewModel from.</param>
        public bool RemovePaymentAmount(PaymentViewModel payment)
        {
            var succeded = RemovePaymentAmount(payment, payment.ChargedAccount);
            return succeded;
        }

        /// <summary>
        ///     Removes the PaymentViewModel Amount from the selected AccountViewModel
        /// </summary>
        /// <param name="payment">PaymentViewModel to remove.</param>
        /// <param name="accountViewModel to remove the amount from.</param>
        public bool RemovePaymentAmount(PaymentViewModel payment, AccountViewModel accountViewModel)
        {
            if (!payment.IsCleared)
            {
                return false;
            }

            PrehandleRemoveIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == PaymentType.Income
                    ? -x
                    : x;

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(accountViewModel));
            return true;
        }

        public bool DeletePayment(PaymentViewModel payment)
        {
            paymentRepository.Delete(payment);

            // If this PaymentViewModel was the last one for the linked recurring PaymentViewModel
            // delete the db entry for the recurring PaymentViewModel.
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

        private void PrehandleRemoveIfTransfer(PaymentViewModel payment)
        {
            if (payment.Type == PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandlePaymentAmount(PaymentViewModel payment,
            Func<double, double> amountFunc,
            Func<PaymentViewModel, AccountViewModel> getAccountFunc)
        {
            var account = getAccountFunc(payment);
            if (account == null)
            {
                return;
            }

            account.CurrentBalance += amountFunc(payment.Amount);
            accountRepository.Save(account);
        }

        private void PrehandleAddIfTransfer(PaymentViewModel payment)
        {
            if (payment.Type == PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<PaymentViewModel, AccountViewModel> GetTargetAccountFunc()
        {
            Func<PaymentViewModel, AccountViewModel> targetAccountFunc =
                trans => accountRepository.GetList(x => x.Id == trans.TargetAccountId).FirstOrDefault();
            return targetAccountFunc;
        }

        private Func<PaymentViewModel, AccountViewModel> GetChargedAccountFunc(AccountViewModel accountViewModel)
        {
            Func<PaymentViewModel, AccountViewModel> accountFunc =
                trans => accountRepository.GetList(x => x.Id == accountViewModel.Id).FirstOrDefault();
            return accountFunc;
        }
    }
}