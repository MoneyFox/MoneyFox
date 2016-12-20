using System;

namespace MoneyFox.Foundation.Models
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
        public StatisticItem Expense { get; set; }

        /// <summary>
        ///     The revenue of the cash flow object.
        /// </summary>
        public StatisticItem Revenue { get; set; }

        /// <summary>
        ///     Describes the Cash Flow
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     The month of the cashflow
        /// </summary>
        public string Month{ get; set; }
    }
}