using MoneyFox.Ui.Shared.Commands;
using System;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    /// <summary>
    /// Represents the Actions for a view.     On Windows this is a normaly in the app bar.     On Android for example
    /// in a floating action button.
    /// </summary>
    public interface IPaymentListViewActionViewModel : IViewActionViewModel
    {
        /// <summary>
        /// Deletes the currently selected account.
        /// </summary>
        AsyncCommand DeleteAccountCommand { get; }

        /// <summary>
        /// Indicates wether the filter for only cleared Payments is active or not.
        /// </summary>
        bool IsClearedFilterActive { get; set; }

        /// <summary>
        /// Indicates wether the filter to only display recurring Payments is active or not.
        /// </summary>
        bool IsRecurringFilterActive { get; set; }

        /// <summary>
        /// Start of the time range to load payments.
        /// </summary>
        DateTime TimeRangeStart { get; set; }

        /// <summary>
        /// End of the time range to load payments.
        /// </summary>
        DateTime TimeRangeEnd { get; set; }
    }
}
