using System.Threading.Tasks;

namespace MoneyFox.ServiceLayer.Interfaces
{
    /// <summary>
    ///     Manages operations with recurring payments
    /// </summary>
    public interface IRecurringPaymentManager
    {
        /// <summary>
        ///     Selects recurring payments who are up for to
        ///     recur and creates new payments for them.
        /// </summary>
        Task CreatePaymentsUpToRecur();
    }
}