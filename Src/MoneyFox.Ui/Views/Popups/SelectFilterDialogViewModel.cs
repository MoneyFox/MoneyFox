namespace MoneyFox.Ui.Views.Popups;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.AccountAggregate;
using Payments;
using Payments.PaymentList;

public interface ISelectFilterDialogViewModel
{
    bool IsClearedFilterActive { get; set; }

    bool IsRecurringFilterActive { get; set; }

    PaymentTypeFilter FilteredPaymentType { get; set; }

    DateTime TimeRangeStart { get; set; }

    DateTime TimeRangeEnd { get; set; }
}

internal sealed class SelectFilterDialogViewModel : BasePageViewModel, ISelectFilterDialogViewModel
{
    private PaymentTypeFilter filteredPaymentType = PaymentTypeFilter.All;
    private bool isClearedFilterActive;
    private bool isRecurringFilterActive;
    private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);
    private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);

    public List<PaymentTypeFilter> PaymentTypeFilterList
        => new()
        {
            PaymentTypeFilter.All,
            PaymentTypeFilter.Expense,
            PaymentTypeFilter.Income,
            PaymentTypeFilter.Transfer
        };

    public RelayCommand FilterSelectedCommand
        => new(
            () => Messenger.Send(
                new PaymentListFilterChangedMessage
                {
                    IsClearedFilterActive = IsClearedFilterActive,
                    IsRecurringFilterActive = IsRecurringFilterActive,
                    TimeRangeStart = TimeRangeStart,
                    TimeRangeEnd = TimeRangeEnd,
                    FilteredPaymentType = FilteredPaymentType
                }));

    /// <summary>
    ///     Indicates whether the filter for only cleared Payments is active or not.
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
        }
    }

    /// <summary>
    ///     Indicates whether the filter to only display a specific payment type is active or not.
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
        }
    }

    /// <summary>
    ///     Initalize the viewmodel with a previous sent message.
    /// </summary>
    public void Initialize(PaymentListFilterChangedMessage message)
    {
        IsClearedFilterActive = message.IsClearedFilterActive;
        IsRecurringFilterActive = message.IsRecurringFilterActive;
        TimeRangeStart = message.TimeRangeStart;
        TimeRangeEnd = message.TimeRangeEnd;
        FilteredPaymentType = message.FilteredPaymentType;
    }
}
