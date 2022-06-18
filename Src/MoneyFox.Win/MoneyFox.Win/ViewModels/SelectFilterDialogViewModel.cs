namespace MoneyFox.Win.ViewModels;

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Messages;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

internal sealed class SelectFilterDialogViewModel : BaseViewModel, ISelectFilterDialogViewModel
{
    private bool isClearedFilterActive;
    private bool isRecurringFilterActive;
    private PaymentTypeFilter filteredPaymentType = PaymentTypeFilter.All;
    private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);
    private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);

    /// <summary>
    ///     Indicates wether the filter for only cleared Payments is active or not.
    /// </summary>
    public bool IsClearedFilterActive
    {
        get => isClearedFilterActive;

        set
        {
            if (isClearedFilterActive == value)
            {
                return;
            }

            isClearedFilterActive = value;
            OnPropertyChanged();
            UpdateList();
        }
    }

    /// <summary>
    ///     Indicates whether the filter to only display recurring Payments is active or not.
    /// </summary>
    public bool IsRecurringFilterActive
    {
        get => isRecurringFilterActive;

        set
        {
            if (isRecurringFilterActive == value)
            {
                return;
            }

            isRecurringFilterActive = value;
            OnPropertyChanged();
            UpdateList();
        }
    }

    /// <summary>
    ///     Indicates whether to filter on specific payment types.
    /// </summary>
    public PaymentTypeFilter FilteredPaymentType
    {
        get => filteredPaymentType;

        set
        {
            if (filteredPaymentType == value)
            {
                return;
            }

            filteredPaymentType = value;
            OnPropertyChanged();
            UpdateList();
        }
    }

    /// <summary>
    ///     Start of the time range to load payments.
    /// </summary>
    public DateTime TimeRangeStart
    {
        get => timeRangeStart;

        set
        {
            if (timeRangeStart == value)
            {
                return;
            }

            timeRangeStart = value;
            OnPropertyChanged();
            UpdateList();
        }
    }

    /// <summary>
    ///     End of the time range to load payments.
    /// </summary>
    public DateTime TimeRangeEnd
    {
        get => timeRangeEnd;

        set
        {
            if (timeRangeEnd == value)
            {
                return;
            }

            timeRangeEnd = value;
            OnPropertyChanged();
            UpdateList();
        }
    }

    private void UpdateList()
    {
        Messenger.Send(
            new PaymentListFilterChangedMessage
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = TimeRangeStart,
                TimeRangeEnd = TimeRangeEnd,
                FilteredPaymentType = FilteredPaymentType
            });
    }
}
