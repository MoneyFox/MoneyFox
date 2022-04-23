namespace MoneyFox.Win.ViewModels;

using System;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

public interface ISelectFilterDialogViewModel
{
    bool IsClearedFilterActive { get; set; }

    bool IsRecurringFilterActive { get; set; }

    PaymentTypeFilter FilteredPaymentType { get; set; }

    DateTime TimeRangeStart { get; set; }

    DateTime TimeRangeEnd { get; set; }
}
