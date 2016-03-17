using System.Linq;
using MoneyFox.Core.Helpers;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Core.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IAccountRepository accountRepository;

        public RecurringPaymentManager(IPaymentRepository paymentRepository, 
            IAccountRepository accountRepository)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
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
                    var newPayment = RecurringPaymentHelper.GetPaymentFromRecurring(payment.RecurringPayment);

                    paymentRepository.Save(newPayment);
                    accountRepository.AddPaymentAmount(newPayment);
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