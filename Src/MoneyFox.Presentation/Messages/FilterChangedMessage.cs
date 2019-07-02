using System;

namespace MoneyFox.Presentation.Messages
{
    /// <summary>
    ///     Used to notify the payment list that a filter changed.
    /// </summary>
    public class PaymentListFilterChangedMessage
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="sender">sender object</param>
        public PaymentListFilterChangedMessage()
        {
        }

        /// <summary>
        ///     Indicates if only cleared payments should be displayed.
        /// </summary>
        public bool IsClearedFilterActive { get; set; }

        /// <summary>
        ///     Indicates if only recurring payments should be displayed.
        /// </summary>
        public bool IsRecurringFilterActive { get; set; }

        /// <summary>
        ///     Start of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeStart { get; set; } = DateTime.Now.AddMonths(-2);

        /// <summary>
        ///     End of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeEnd { get; set; } = DateTime.MaxValue;
    }
}
