namespace MoneyFox.Win.ViewModels.DesignTime;

using Core.Aggregates.Payments;
using System;

public class DesignTimeSelectFilterDialogViewModel : ISelectFilterDialogViewModel
{
    public bool IsClearedFilterActive { get; set; }

    public bool IsRecurringFilterActive { get; set; }

    public PaymentTypeFilter FilteredPaymentType { get; set; }

    public DateTime TimeRangeStart { get; set; }

    public DateTime TimeRangeEnd { get; set; }
}