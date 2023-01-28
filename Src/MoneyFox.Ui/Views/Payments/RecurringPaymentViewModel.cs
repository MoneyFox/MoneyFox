namespace MoneyFox.Ui.ViewModels.Payments;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.ApplicationCore.Domain.Aggregates;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Common.Helpers;
using Core.Common.Interfaces.Mapping;
using Views.Accounts;
using Views.Categories;

public class RecurringPaymentViewModel : ObservableObject, IHaveCustomMapping
{
    private const decimal DECIMAL_DELTA = 0.01m;
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

        set
        {
            if (id == value)
            {
                return;
            }

            id = value;
            OnPropertyChanged();
        }
    }

    public DateTime StartDate
    {
        get => startDate;

        set
        {
            if (startDate == value)
            {
                return;
            }

            startDate = value;
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

    public DateTime? EndDate
    {
        get => endDate;

        set
        {
            if (endDate == value)
            {
                return;
            }

            endDate = value;
            OnPropertyChanged();
        }
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
            EndDate = isEndless is false ? DateTime.Today : null;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
    /// </summary>
    public decimal Amount
    {
        get => amount;

        set
        {
            if (Math.Abs(amount - value) < DECIMAL_DELTA)
            {
                return;
            }

            amount = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Type of the payment <see cref="PaymentType" />.
    /// </summary>
    public PaymentType Type
    {
        get => type;

        set
        {
            if (type == value)
            {
                return;
            }

            type = value;
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
    public bool AllowLastDayOfMonth => RecurringPaymentHelper.AllowLastDayOfMonth(Recurrence);

    /// <summary>
    ///     Additional notes to the payment.
    /// </summary>
    public string Note
    {
        get => note;

        set
        {
            if (note == value)
            {
                return;
            }

            note = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     In case it's a expense or transfer the account who will be charged.     In case it's an income the account
    ///     who will be credited.
    /// </summary>
    public AccountViewModel ChargedAccount
    {
        get => chargedAccount;

        set
        {
            if (chargedAccount == value)
            {
                return;
            }

            chargedAccount = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     The <see cref="Category" /> for this payment
    /// </summary>
    public CategoryListItemViewModel? Category
    {
        get => categoryViewModel;

        set
        {
            if (categoryViewModel == value)
            {
                return;
            }

            categoryViewModel = value;
            OnPropertyChanged();
        }
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<RecurringPayment, RecurringPaymentViewModel>();
    }
}
