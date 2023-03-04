namespace MoneyFox.Ui.Views.Payments;

using Accounts;
using AutoMapper;
using Categories;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;

public class RecurringPaymentViewModel : ObservableViewModelBase, IHaveCustomMapping
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
        set => SetProperty(property: ref id, value: value);
    }

    public DateTime StartDate
    {
        get => startDate;

        set => SetProperty(property: ref startDate, value: value);
    }

    public bool IsLastDayOfMonth
    {
        get => isLastDayOfMonth;
        set => SetProperty(property: ref isLastDayOfMonth, value: value);
    }

    public DateTime? EndDate
    {
        get => endDate;
        set => SetProperty(property: ref endDate, value: value);
    }

    public bool IsEndless
    {
        get => isEndless;

        set
        {
            SetProperty(property: ref isEndless, value: value);
            EndDate = isEndless is false ? DateTime.Today : null;
            RaisePropertyChanged(nameof(isEndless));
        }
    }

    /// <summary>
    ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
    /// </summary>
    public decimal Amount
    {
        get => amount;
        set => SetProperty(property: ref amount, value: value);
    }

    /// <summary>
    ///     Type of the payment <see cref="PaymentType" />.
    /// </summary>
    public PaymentType Type
    {
        get => type;
        set => SetProperty(property: ref type, value: value);
    }

    public PaymentRecurrence Recurrence
    {
        get => recurrence;

        set
        {
            SetProperty(property: ref recurrence, value: value);

            // If recurrence is changed to a type that doesn't allow the Last Day of Month flag, force the flag to false if it's true
            if (IsLastDayOfMonth && !AllowLastDayOfMonth)
            {
                IsLastDayOfMonth = false;
            }

            RaisePropertyChanged(nameof(AllowLastDayOfMonth));
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
        set => SetProperty(property: ref note, value: value);
    }

    /// <summary>
    ///     In case it's a expense or transfer the account who will be charged.     In case it's an income the account
    ///     who will be credited.
    /// </summary>
    public AccountViewModel ChargedAccount
    {
        get => chargedAccount;
        set => SetProperty(property: ref chargedAccount, value: value);
    }

    /// <summary>
    ///     The <see cref="Category" /> for this payment
    /// </summary>
    public CategoryListItemViewModel? Category
    {
        get => categoryViewModel;
        set => SetProperty(property: ref categoryViewModel, value: value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<RecurringPayment, RecurringPaymentViewModel>();
    }
}
