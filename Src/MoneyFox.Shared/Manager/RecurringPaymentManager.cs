using System;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IPaymentManager paymentManager;
        private readonly IRepository<Payment> paymentRepository;

        public RecurringPaymentManager(IPaymentManager paymentManager, IRepository<Payment> paymentRepository)
        {
            this.paymentManager = paymentManager;
            this.paymentRepository = paymentRepository;
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

                    var paymentSucceded = paymentRepository.Save(newPayment);
                    var accountSucceded = paymentManager.AddPaymentAmount(newPayment);
                    if (paymentSucceded && accountSucceded)
                        SettingsHelper.LastDatabaseUpdate = DateTime.Now;
                }
            }
        }

        private Payment GetLastOccurence(Payment payment)
        {
            var transcationList = paymentRepository.Data
                .Where(x => x.RecurringPaymentId == payment.RecurringPaymentId)
                .OrderBy(x => x.Date)
                .ToList();

            return transcationList.LastOrDefault();
        }
    }
}