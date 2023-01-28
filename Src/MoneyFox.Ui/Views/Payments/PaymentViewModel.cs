namespace MoneyFox.Ui.Views.Payments;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.Common.Interfaces.Mapping;
using MoneyFox.Ui.Views.Accounts;
using MoneyFox.Ui.Views.Categories;

public class PaymentViewModel : ObservableObject, IHaveCustomMapping
{
    private const decimal DECIMAL_DELTA = 0.01m;
    private decimal amount;
    private CategoryListItemViewModel? categoryViewModel;

    private AccountViewModel chargedAccount = null!;
    private int chargedAccountId;
    private DateTime created;

    private int currentAccountId;
    private DateTime date;

    private int id;
    private bool isCleared;
    private bool isRecurring;
    private DateTime lastModified;
    private string note = "";
    private RecurringPaymentViewModel? recurringPaymentViewModel;
    private AccountViewModel? targetAccount;
    private int? targetAccountId;
    private PaymentType type;

    public PaymentViewModel()
    {
        Date = DateTime.Today;
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

    /// <summary>
    ///     In case it's a expense or transfer the foreign key to the <see cref="AccountViewModel" /> who will be
    ///     charged.     In case it's an income the  foreign key to the <see cref="AccountViewModel" /> who will be
    ///     credited.
    /// </summary>
    public int ChargedAccountId
    {
        get => chargedAccountId;

        set
        {
            if (chargedAccountId == value)
            {
                return;
            }

            chargedAccountId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Foreign key to the account who will be credited by a transfer.     Not used for the other payment types.
    /// </summary>
    public int? TargetAccountId
    {
        get => targetAccountId;

        set
        {
            if (targetAccountId == value)
            {
                return;
            }

            targetAccountId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Date when this payment will be executed.
    /// </summary>
    public DateTime Date
    {
        get => date;

        set
        {
            if (date == value)
            {
                return;
            }

            date = value;
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
    ///     Indicates if this payment was already executed and the amount already credited or charged to the respective
    ///     account.
    /// </summary>
    public bool IsCleared
    {
        get => isCleared;

        set
        {
            if (isCleared == value)
            {
                return;
            }

            isCleared = value;
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
            OnPropertyChanged(nameof(IsTransfer));
        }
    }

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
    ///     Indicates if the payment will be repeated or if it's a uniquie payment.
    /// </summary>
    public bool IsRecurring
    {
        get => isRecurring;

        set
        {
            if (isRecurring == value)
            {
                return;
            }

            isRecurring = value;
            RecurringPayment = isRecurring ? new RecurringPaymentViewModel() : null;
            OnPropertyChanged();
        }
    }

    public DateTime Created
    {
        get => created;

        set
        {
            if (created == value)
            {
                return;
            }

            created = value;
            OnPropertyChanged();
        }
    }

    public DateTime LastModified
    {
        get => lastModified;

        set
        {
            if (lastModified == value)
            {
                return;
            }

            lastModified = value;
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
    ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.     Not used for the other payment
    ///     types.
    /// </summary>
    public AccountViewModel? TargetAccount
    {
        get => targetAccount;

        set
        {
            if (targetAccount == value)
            {
                return;
            }

            targetAccount = value;
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

    /// <summary>
    ///     The <see cref="RecurringPayment" /> if it's recurring.
    /// </summary>
    public RecurringPaymentViewModel? RecurringPayment
    {
        get => recurringPaymentViewModel;

        set
        {
            if (recurringPaymentViewModel == value)
            {
                return;
            }

            recurringPaymentViewModel = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     This is a shortcut to access if the payment is a transfer or not.
    /// </summary>
    public bool IsTransfer => Type == PaymentType.Transfer;

    /// <summary>
    ///     Id of the account who currently is used for that view.
    /// </summary>
    public int CurrentAccountId
    {
        get => currentAccountId;

        set
        {
            if (currentAccountId == value)
            {
                return;
            }

            currentAccountId = value;
            OnPropertyChanged();
        }
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Payment, PaymentViewModel>()
            .ForMember(destinationMember: x => x.CurrentAccountId, memberOptions: opt => opt.Ignore())
            .ReverseMap();
    }
}
