namespace MoneyFox.Win.ViewModels.Payments;

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Aggregates.Payments;
using Core.Common.Interfaces.Mapping;

public class RecurringPaymentViewModel : ObservableObject, IMapFrom<RecurringPayment>
{
    private DateTime? endDate;
    private int id;
    private bool isEndless;
    private PaymentRecurrence recurrence;

    public RecurringPaymentViewModel()
    {
        Recurrence = PaymentRecurrence.Daily;
        EndDate = null;
        IsEndless = true;
    }

    public int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    public DateTime? EndDate
    {
        get => endDate;
        set => SetProperty(field: ref endDate, newValue: value);
    }

    public bool IsEndless
    {
        get => isEndless;

        set
        {
            if (isEndless == value)
            {
                return;
            }

            isEndless = value;
            EndDate = isEndless is false ? EndDate = DateTime.Today : null;
            OnPropertyChanged();
        }
    }

    public PaymentRecurrence Recurrence
    {
        get => recurrence;
        set => SetProperty(field: ref recurrence, newValue: value);
    }
}
