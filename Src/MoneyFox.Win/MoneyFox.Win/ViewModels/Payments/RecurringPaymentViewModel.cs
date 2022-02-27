namespace MoneyFox.Win.ViewModels.Payments;

using CommunityToolkit.Mvvm.ComponentModel;
using Core._Pending_.Common.Interfaces.Mapping;
using Core.Aggregates.Payments;
using System;

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
        set => SetProperty(ref id, value);
    }

    public DateTime? EndDate
    {
        get => endDate;
        set => SetProperty(ref endDate, value);
    }

    public bool IsEndless
    {
        get => isEndless;
        set
        {
            if(isEndless == value)
            {
                return;
            }

            isEndless = value;
            EndDate = isEndless is false
                ? EndDate = DateTime.Today
                : null;

            OnPropertyChanged();
        }
    }

    public PaymentRecurrence Recurrence
    {
        get => recurrence;
        set => SetProperty(ref recurrence, value);
    }
}