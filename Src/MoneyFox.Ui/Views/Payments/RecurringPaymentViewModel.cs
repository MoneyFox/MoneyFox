namespace MoneyFox.Ui.Views.Payments;

using Accounts.AccountModification;
using AutoMapper;
using Categories;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;

public class RecurringPaymentViewModel : ObservableObject, IHaveCustomMapping
{
    private decimal amount;
    private CategoryListItemViewModel? categoryViewModel;

    private DateTime? endDate;

    private int id;
    private bool isEndless;
    private bool isLastDayOfMonth;
    private string note = "";
    private PaymentRecurrence recurrence;
    private DateTime startDate;
    private PaymentType type;
    private int chargedAccountId;

    public RecurringPaymentViewModel()
    {
        Recurrence = PaymentRecurrence.Daily;
        IsLastDayOfMonth = false;
        EndDate = null;
        IsEndless = true;
    }

    public int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    public DateTime StartDate
    {
        get => startDate;

        set => SetProperty(field: ref startDate, newValue: value);
    }

    public bool IsLastDayOfMonth
    {
        get => isLastDayOfMonth;
        set => SetProperty(field: ref isLastDayOfMonth, newValue: value);
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
            SetProperty(field: ref isEndless, newValue: value);
            EndDate = isEndless is false ? DateTime.Today : null;
            OnPropertyChanged(nameof(isEndless));
        }
    }

    /// <summary>
    ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
    /// </summary>
    public decimal Amount
    {
        get => amount;
        set => SetProperty(field: ref amount, newValue: value);
    }

    /// <summary>
    ///     Type of the payment <see cref="PaymentType" />.
    /// </summary>
    public PaymentType Type
    {
        get => type;
        set => SetProperty(field: ref type, newValue: value);
    }

    public PaymentRecurrence Recurrence
    {
        get => recurrence;

        set => SetProperty(field: ref recurrence, newValue: value);
    }

    /// <summary>
    ///     The <see cref="Category" /> for this payment
    /// </summary>
    public CategoryListItemViewModel? Category
    {
        get => categoryViewModel;
        set => SetProperty(field: ref categoryViewModel, newValue: value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<RecurringPayment, RecurringPaymentViewModel>().ReverseMap();
    }
}
