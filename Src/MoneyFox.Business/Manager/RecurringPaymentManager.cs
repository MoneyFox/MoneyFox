using System.Linq;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.Manager
{
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IRecurringPaymentService recurringPaymentService;
        private readonly IPaymentService paymentService;

        public RecurringPaymentManager(IRecurringPaymentService recurringPaymentService, IPaymentService paymentService)
        {
            this.recurringPaymentService = recurringPaymentService;
            this.paymentService = paymentService;
        }

        /// <summary>
        ///     Checks if one of the recurring PaymentViewModel has to be repeated
        /// </summary>
        public async void CreatePaymentsUpToRecur()
        {
            var newPayments = recurringPaymentService.GetPaymentsToRecur()
                .Select(RecurringPaymentHelper.GetPaymentFromRecurring);

            await paymentService.SavePayments(newPayments);
        }
    }
}