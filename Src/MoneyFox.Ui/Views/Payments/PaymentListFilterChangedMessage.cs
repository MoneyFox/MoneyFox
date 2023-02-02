namespace MoneyFox.Ui.Views.Payments;

using System;
using MoneyFox.Domain.Aggregates.AccountAggregate;

/// <summary>
///     Used to notify the payment list that a filter changed.
/// </summary>
public class PaymentListFilterChangedMessage
{
    private const int DEFAULT_MONTHS_BACK = -2;

    /// <summary>
    ///     Indicates if only cleared payments should be displayed.
    /// </summary>
    public bool IsClearedFilterActive { get; set; }

    /// <summary>
    ///     Indicates if only recurring payments should be displayed.
    /// </summary>
    public bool IsRecurringFilterActive { get; set; }

    /// <summary>
    ///     Indicate if payment list should be grouped.
    /// </summary>
    public bool IsGrouped { get; set; }

    /// <summary>
    ///     Indicates if only specific payment types should be displayed.
    /// </summary>
    public PaymentTypeFilter FilteredPaymentType { get; set; } = PaymentTypeFilter.All;

    /// <summary>
    ///     Start of the time range to load payments.
    /// </summary>
    public DateTime TimeRangeStart { get; set; } = DateTime.Now.AddMonths(DEFAULT_MONTHS_BACK);

    /// <summary>
    ///     End of the time range to load payments.
    /// </summary>
    public DateTime TimeRangeEnd { get; set; } = DateTime.MaxValue;
}
