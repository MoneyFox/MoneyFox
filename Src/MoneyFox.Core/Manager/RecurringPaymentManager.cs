using System.Linq;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IPaymentRepository paymentRepository;

        public RecurringPaymentManager(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        ///     Checks if one of the recurring payment has to be repeated
        /// </summary>
        public void CheckRecurringPayments()
        {
            var paymentList = paymentRepository.LoadRecurringList();

            foreach (var payment in paymentList.Where(x => x.ChargedAccount != null))
            {
                var relatedPayment = GetLastOccurence(payment);

                if (RecurringPaymentHelper.CheckIfRepeatable(payment.RecurringPayment, relatedPayment))
                {
                    paymentRepository.Save(
                        RecurringPaymentHelper.GetPaymentFromRecurring(payment.RecurringPayment));
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