using MoneyManager.Foundation.Model;

namespace MoneyFox.Foundation.Model
{
    /// <summary>
    ///     Represents a cash flow object for usage in statistics
    /// </summary>
    public class CashFlow
    {
        /// <summary>
        ///     The income of the cash flow object.
        /// </summary>
        public StatisticItem Income { get; set; }

        /// <summary>
        ///     The spending of the cash flow object.
        /// </summary>
        public StatisticItem Spending { get; set; }

        /// <summary>
        ///     The revenue of the cash flow object.
        /// </summary>
        public StatisticItem Revenue { get; set; }
    }
}