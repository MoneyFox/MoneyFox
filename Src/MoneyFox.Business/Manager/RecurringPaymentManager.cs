using System;
using System.Linq;
using MoneyFox.Business.Helpers;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly ISettingsManager settingsManager;

        public RecurringPaymentManager(IPaymentManager paymentManager, IPaymentRepository paymentRepository,
            ISettingsManager settingsManager)
        {
            this.paymentManager = paymentManager;
            this.paymentRepository = paymentRepository;
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Checks if one of the recurring PaymentViewModel has to be repeated
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
                    {
                        settingsManager.LastDatabaseUpdate = DateTime.Now;
                    }
                }
            }
        }

        private PaymentViewModel GetLastOccurence(PaymentViewModel payment)
        {
            var transcationList = paymentRepository
                .GetList(x => x.RecurringPaymentId == payment.RecurringPaymentId)
                .OrderBy(x => x.Date)
                .ToList();

            return transcationList.LastOrDefault();
        }
    }
}