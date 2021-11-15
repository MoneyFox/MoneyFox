using CommunityToolkit.Mvvm.Input;
using System;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    /// <summary>
    ///     Represents the Actions for a view.
    /// </summary>
    public interface IPaymentListViewActionViewModel
    {
        /// <summary>
        ///     Deletes the currently selected account.
        /// </summary>
        AsyncRelayCommand DeleteAccountCommand { get; }

        /// <summary>
        ///     Apply the filter and reload the list.
        /// </summary>
        RelayCommand ApplyFilterCommand { get; }

        /// <summary>
        ///     Indicates wether the filter for only cleared Payments is active or not.
        /// </summary>
        bool IsClearedFilterActive { get; set; }

        /// <summary>
        ///     Indicates wether the filter to only display recurring Payments is active or not.
        /// </summary>
        bool IsRecurringFilterActive { get; set; }

        /// <summary>
        ///     Indicates wether the paymentlist should be grouped
        /// </summary>
        bool IsGrouped { get; set; }

        /// <summary>
        ///     Start of the time range to load payments.
        /// </summary>
        DateTime TimeRangeStart { get; set; }

        /// <summary>
        ///     End of the time range to load payments.
        /// </summary>
        DateTime TimeRangeEnd { get; set; }
    }
}