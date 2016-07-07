using System;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IPaymentManager paymentManager;
        private readonly IUnitOfWork unitOfWork;

        public RecurringPaymentManager(IUnitOfWork unitOfWork,
            IPaymentManager paymentManager)
        {
            this.unitOfWork = unitOfWork;
            this.paymentManager = paymentManager;
        }

        /// <summary>
        ///     Checks if one of the recurring payment has to be repeated
        /// </summary>
        public void CheckRecurringPayments()
        {
            var paymentList = paymentManager.LoadRecurringPaymentList();

            foreach (var payment in paymentList.Where(x => x.ChargedAccount != null))
            {
                var relatedPayment = GetLastOccurence(payment);

                if (RecurringPaymentHelper.CheckIfRepeatable(payment.RecurringPayment, relatedPayment))
                {
                    var newPayment = RecurringPaymentHelper.GetPaymentFromRecurring(payment.RecurringPayment);

                    var paymentSucceded = unitOfWork.PaymentRepository.Save(newPayment);
                    var accountSucceded = paymentManager.AddPaymentAmount(newPayment);
                    if (paymentSucceded && accountSucceded)
                        SettingsHelper.LastDatabaseUpdate = DateTime.Now;
                }
            }
        }

        private Payment GetLastOccurence(Payment payment)
        {
            var transcationList = unitOfWork.PaymentRepository.Data
                .Where(x => x.RecurringPaymentId == payment.RecurringPaymentId)
                .OrderBy(x => x.Date)
                .ToList();

            return transcationList.LastOrDefault();
        }
    }
}