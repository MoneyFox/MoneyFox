namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.AccountAggregate;

public class RecurrenceViewModel : ObservableObject

{
    private DateTime? endDate;
    private bool isEndless;
    private bool isLastDayOfMonth;
    private PaymentRecurrence recurrence;
    private DateTime startDate;

    public PaymentRecurrence Recurrence
    {
        get => recurrence;

        set
        {
            SetProperty(field: ref recurrence, newValue: value);
            if (AllowLastDayOfMonth is false)
            {
                IsLastDayOfMonth = false;
            }
        }
    }

    public DateTime StartDate
    {
        get => startDate;
        set => SetProperty(field: ref startDate, newValue: value);
    }

    public DateTime? EndDate
    {
        get => endDate;
        set => SetProperty(field: ref endDate, newValue: value);
    }

    public bool IsLastDayOfMonth
    {
        get => isLastDayOfMonth;
        set => SetProperty(field: ref isLastDayOfMonth, newValue: value);
    }

    public bool IsEndless
    {
        get => isEndless;

        set
        {
            SetProperty(field: ref isEndless, newValue: value);
            EndDate = isEndless is false ? DateTime.Today : null;
            OnPropertyChanged(nameof(isEndless));
        }
    }

    public bool AllowLastDayOfMonth
        => Recurrence switch
        {
            PaymentRecurrence.Monthly
                or PaymentRecurrence.Bimonthly
                or PaymentRecurrence.Quarterly
                or PaymentRecurrence.Biannually
                or PaymentRecurrence.Yearly => true,
            _ => false
        };
}
