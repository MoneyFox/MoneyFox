using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.Manager
{
    /// <inheritdoc />
    public class RecurringPaymentManager : IRecurringPaymentManager
    {
        private readonly IRecurringPaymentService recurringPaymentService;
        private readonly IPaymentService paymentService;

        /// <summary>
        ///     Contstructor
        /// </summary>
        public RecurringPaymentManager(IRecurringPaymentService recurringPaymentService, IPaymentService paymentService)
        {
            this.recurringPaymentService = recurringPaymentService;
            this.paymentService = paymentService;
        }

        /// <inheritdoc />
        public async Task CreatePaymentsUpToRecur()
        {
            var newPayments = await recurringPaymentService.GetPaymentsToRecur();
            await paymentService.SavePayments(newPayments.Select(RecurringPaymentHelper.GetPaymentFromRecurring).ToArray());
        }
    }
}