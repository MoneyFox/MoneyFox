using System;
using System.Linq;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.Manager
{
    /// <summary>
    ///     Manager to handle payment clearing.
    /// </summary>
    public interface IClearPaymentManager
    {
        /// <summary>
        ///     Clears all payments up for clearing.
        /// </summary>
        void ClearPayments();
    }

    /// <inheritdoc />
    public class ClearPaymentManager : IClearPaymentManager
    {
        private readonly IPaymentService paymentService;

        public ClearPaymentManager(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        /// <inheritdoc />
        public async void ClearPayments()
        {
            var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
            var paymentList = payments.ToList();

            foreach (var payment in paymentList)
            {
                payment.ClearPayment();
            }

            await paymentService.SavePayments(paymentList);
        }
    }
}
