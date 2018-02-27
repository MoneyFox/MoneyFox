using System;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.Messages
{
    /// <summary>
    ///     Used to notify the payment list that a filter changed.
    /// </summary>
    public class PaymentListFilterChangedMessage : MvxMessage
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="sender">sender object</param>
        public PaymentListFilterChangedMessage(object sender) : base(sender)
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
        public DateTime TimeRangeStart { get; set; } = DateTime.MaxValue;

        /// <summary>
        ///     End of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeEnd { get; set; } = DateTime.Now.AddMonths(-2);
    }
}
