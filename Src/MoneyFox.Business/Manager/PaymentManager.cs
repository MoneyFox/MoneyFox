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
                payment.RecurringPayment = null;
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

        /// <summary>
        ///     Deletes all the payments and the recurring payments associated with the account.
        /// </summary>
        /// <param name="accountViewModel">The associated Account.</param>
        public void DeleteAssociatedPaymentsFromDatabase(AccountViewModel accountViewModel)
        {
            var paymentToDelete = paymentRepository
                .GetList(x => (x.ChargedAccountId == accountViewModel.Id) || (x.TargetAccountId == accountViewModel.Id))
                .OrderByDescending(x => x.Date)
                .ToList();

            foreach (var payment in paymentToDelete)
            {
                if (payment.IsRecurring)
                {
                    foreach (var recTrans in recurringPaymentRepository.GetList(x => x.Id == payment.RecurringPaymentId)
                    )
                    {
                        recurringPaymentRepository.Delete(recTrans);
                    }
                }

                paymentRepository.Delete(payment);
            }
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

            var recurringPayments = recurringPaymentRepository.GetList().Select(x => x.Id).ToList();

            var clearedRecPayments = list.Where(x => recurringPayments.Contains(x.RecurringPaymentId)).ToList();

            var listLastOccurence = new List<PaymentViewModel>();

            foreach (var recPaymentId in recurringPayments)
            {
                var payment = clearedRecPayments
                    .Where(x => x.RecurringPaymentId == recPaymentId)
                    .OrderByDescending(x => x.Date)
                    .LastOrDefault();

                if(payment == null) continue;

                listLastOccurence.Add(payment);
            }

            return listLastOccurence;
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
        /// <param name="accountViewModel">AccountViewModel to remove the amount from.</param>
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

        /// <summary>
        ///     Deletes a payment and if asks the user if the recurring payment shall be deleted as well.
        ///     If the users says yes, delete recurring payment.
        /// </summary>
        /// <param name="payment">Payment to delete.</param>
        /// <returns>Returns if the operation was successful</returns>
        public async Task<bool> DeletePayment(PaymentViewModel payment)
        {
            paymentRepository.Delete(payment);

            if (!payment.IsRecurring || !await
                    dialogService.ShowConfirmMessage(Strings.DeleteRecurringPaymentTitle,
                        Strings.DeleteRecurringPaymentMessage)) return true;

            // Delete all recurring payments if the user wants so.
            return DeleteRecurringPaymentForPayment(payment);
        }

        private bool DeleteRecurringPaymentForPayment(PaymentViewModel payment)
        {
            var succeed = true;
            if (recurringPaymentRepository.GetList().Any(x => x.Id == payment.RecurringPaymentId))
            {
                var recpayment = recurringPaymentRepository
                    .FindById(payment.RecurringPaymentId);

                if (!recurringPaymentRepository.Delete(recpayment))
                {
                    succeed = false;
                }

                // edit other payments
                if (succeed)
                {
                    foreach (var paymentViewModel in
                        paymentRepository.GetList(x => x.IsRecurring && x.RecurringPaymentId == recpayment.Id))
                    {
                        paymentViewModel.IsRecurring = false;
                        paymentViewModel.RecurringPayment = null;

                        var saveSuccess = paymentRepository.Save(paymentViewModel);

                        if (!saveSuccess)
                        {
                            succeed = false;
                        }
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