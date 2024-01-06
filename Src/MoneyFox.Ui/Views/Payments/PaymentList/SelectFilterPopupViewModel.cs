namespace MoneyFox.Ui.Views.Payments.PaymentList;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.AccountAggregate;

public sealed class SelectFilterPopupViewModel(IMessenger messenger) : NavigableViewModel
{
    private PaymentTypeFilter filteredPaymentType = PaymentTypeFilter.All;
    private bool isClearedFilterActive;
    private bool isRecurringFilterActive;
    private DateTime timeRangeEnd = DateTime.Today.AddMonths(6);
    private DateTime timeRangeStart = DateTime.Today.AddMonths(-2);

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
            () => messenger.Send(
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
            OnPropertyChanged(nameof(IsDateRangeValid));
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
            OnPropertyChanged(nameof(IsDateRangeValid));
        }
    }

    public bool IsDateRangeValid => TimeRangeStart <= TimeRangeEnd;

    /// <summary>
    ///     Initialize the viewmodel with a previous sent message.
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
