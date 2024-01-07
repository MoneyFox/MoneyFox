namespace MoneyFox.Ui.Views.Payments.PaymentList;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.AccountAggregate;

public sealed class SelectFilterPopupViewModel : ObservableRecipient
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
            () => Messenger.Send(
                new PaymentListFilterChangedMessage
                {
                    IsClearedFilterActive = IsClearedFilterActive,
                    IsRecurringFilterActive = IsRecurringFilterActive,
                    TimeRangeStart = TimeRangeStart,
                    TimeRangeEnd = TimeRangeEnd,
                    FilteredPaymentType = FilteredPaymentType
                }));

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

    public void Initialize(PaymentListFilterChangedMessage? message)
    {
        if (message == null)
        {
            return;
        }
        
        IsClearedFilterActive = message.IsClearedFilterActive;
        IsRecurringFilterActive = message.IsRecurringFilterActive;
        TimeRangeStart = message.TimeRangeStart;
        TimeRangeEnd = message.TimeRangeEnd;
        FilteredPaymentType = message.FilteredPaymentType;
    }
}
