using System;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly ISettingsManager settingsManager;

        public RecurringPaymentManager(IPaymentManager paymentManager, IPaymentRepository paymentRepository, ISettingsManager settingsManager)
        {
            this.paymentManager = paymentManager;
            this.paymentRepository = paymentRepository;
            this.settingsManager = settingsManager;
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
                        settingsManager.LastDatabaseUpdate = DateTime.Now;
                }
            }
        }

        private Payment GetLastOccurence(Payment payment)
        {
            var transcationList = paymentRepository
                .GetList(x => x.RecurringPaymentId == payment.RecurringPaymentId)
                .OrderBy(x => x.Date)
                .ToList();

            return transcationList.LastOrDefault();
        }
    }
}