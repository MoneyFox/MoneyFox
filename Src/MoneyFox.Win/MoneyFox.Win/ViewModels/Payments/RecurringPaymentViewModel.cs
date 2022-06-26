namespace MoneyFox.Win.ViewModels.Payments;

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common.Interfaces.Mapping;
using Core.ApplicationCore.Domain.Aggregates;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Common.Helpers;

public class RecurringPaymentViewModel : ObservableObject, IMapFrom<RecurringPayment>
{
    private DateTime? endDate;
    private int id;
    private bool isEndless;
    private bool isLastDayOfMonth;
    private PaymentRecurrence recurrence;

    public RecurringPaymentViewModel()
    {
        Recurrence = PaymentRecurrence.Daily;
        EndDate = null;
        IsEndless = true;
        isLastDayOfMonth = false;
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

    public bool IsLastDayOfMonth
    {
        get => isLastDayOfMonth;
        set
        {
            if (isLastDayOfMonth == value)
            {
                return;
            }

            isLastDayOfMonth = value;
            OnPropertyChanged();
        }
    }
    public PaymentRecurrence Recurrence
    {
        get => recurrence;
        set
        {
            if (recurrence == value)
            {
                return;
            }

            recurrence = value;

            // If recurrence is changed to a type that doesn't allow the Last Day of Month flag, force the flag to false if it's true
            if (IsLastDayOfMonth && !AllowLastDayOfMonth)
            {
                IsLastDayOfMonth = false;
            }

            OnPropertyChanged();

            // Notify that the calculated AllowLastDayOfMonth property has changed so the control visibility is refreshed.
            OnPropertyChanged("AllowLastDayOfMonth");
        }
    }

    /// <summary>
    ///     Boolean indicating whether or not the Last Day Of Month option should be permitted
    /// </summary>
    public bool AllowLastDayOfMonth
    {
        get => RecurringPaymentHelper.AllowLastDayOfMonth(Recurrence);
    }


}
