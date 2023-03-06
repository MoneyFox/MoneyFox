namespace MoneyFox.Ui.Views.Payments;

using Accounts;
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

    private AccountViewModel chargedAccount = null!;
    private DateTime? endDate;

    private int id;
    private bool isEndless;
    private bool isLastDayOfMonth;
    private string note = "";
    private PaymentRecurrence recurrence;
    private DateTime startDate;
    private PaymentType type;

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
        set => SetProperty(ref id, value);
    }

    public DateTime StartDate
    {
        get => startDate;

        set => SetProperty(ref startDate, value);
    }

    public bool IsLastDayOfMonth
    {
        get => isLastDayOfMonth;
        set => SetProperty(ref isLastDayOfMonth, value);
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
            SetProperty(ref isEndless, value);
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
        set => SetProperty(ref amount, value);
    }

    /// <summary>
    ///     Type of the payment <see cref="PaymentType" />.
    /// </summary>
    public PaymentType Type
    {
        get => type;
        set => SetProperty(ref type, value);
    }

    public PaymentRecurrence Recurrence
    {
        get => recurrence;

        set
        {
            SetProperty(ref recurrence, value);

            // If recurrence is changed to a type that doesn't allow the Last Day of Month flag, force the flag to false if it's true
            if (IsLastDayOfMonth && !AllowLastDayOfMonth)
            {
                IsLastDayOfMonth = false;
            }

            OnPropertyChanged(nameof(AllowLastDayOfMonth));
        }
    }

    /// <summary>
    ///     Boolean indicating whether or not the Last Day Of Month option should be permitted
    /// </summary>
    public bool AllowLastDayOfMonth => RecurringPaymentHelper.AllowLastDayOfMonth(Recurrence);

    /// <summary>
    ///     Additional notes to the payment.
    /// </summary>
    public string Note
    {
        get => note;
        set => SetProperty(ref note, value);
    }

    /// <summary>
    ///     In case it's a expense or transfer the account who will be charged.     In case it's an income the account
    ///     who will be credited.
    /// </summary>
    public AccountViewModel ChargedAccount
    {
        get => chargedAccount;
        set => SetProperty(ref chargedAccount, value);
    }

    /// <summary>
    ///     The <see cref="Category" /> for this payment
    /// </summary>
    public CategoryListItemViewModel? Category
    {
        get => categoryViewModel;
        set => SetProperty(ref categoryViewModel, value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<RecurringPayment, RecurringPaymentViewModel>();
    }
}
