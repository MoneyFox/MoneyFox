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
        /// <param name="isClearedFilterActive">Value who shall be passed.</param>
        /// <param name="isRecurringFilterActive">Value who shall be passed.</param>
        public PaymentListFilterChangedMessage(object sender, bool isClearedFilterActive = false, bool isRecurringFilterActive = false) : base(sender)
        {
            IsClearedFilterActive = isClearedFilterActive;
            IsRecurringFilterActive = isRecurringFilterActive;
        }

        /// <summary>
        ///     Indicates if only cleared payments should be displayed.
        /// </summary>
        public bool IsClearedFilterActive { get; set; }

        /// <summary>
        ///     Indicates if only recurring payments should be displayed.
        /// </summary>
        public bool IsRecurringFilterActive { get; set; }
    }
}
