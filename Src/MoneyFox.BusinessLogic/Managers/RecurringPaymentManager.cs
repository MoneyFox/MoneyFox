using System.Linq;
using System.Threading.Tasks;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Interfaces;

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