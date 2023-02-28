namespace MoneyFox.Ui.Views.Payments.PaymentList;

using Accounts;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using PaymentModification;

public class PaymentListItemViewModel : ObservableObject, IHaveCustomMapping
{
    private decimal amount;
    private SelectedCategoryViewModel? categoryViewModel;

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

    public PaymentListItemViewModel()
    {
        Date = DateTime.Today;
    }

    public int Id
    {
        get => id;
        set => SetProperty( ref id,   value);
    }

    /// <summary>
    ///     In case it's a expense or transfer the foreign key to the <see cref="AccountViewModel" /> who will be
    ///     charged.     In case it's an income the  foreign key to the <see cref="AccountViewModel" /> who will be
    ///     credited.
    /// </summary>
    public int ChargedAccountId
    {
        get => chargedAccountId;
        set => SetProperty( ref chargedAccountId,   value);
    }

    /// <summary>
    ///     Foreign key to the account who will be credited by a transfer.     Not used for the other payment types.
    /// </summary>
    public int? TargetAccountId
    {
        get => targetAccountId;
        set => SetProperty( ref targetAccountId,   value);
    }

    /// <summary>
    ///     Date when this payment will be executed.
    /// </summary>
    public DateTime Date
    {
        get => date;
        set => SetProperty( ref date,   value);
    }

    /// <summary>
    ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
    /// </summary>
    public decimal Amount
    {
        get => amount;
        set => SetProperty( ref amount,   value);
    }

    /// <summary>
    ///     Indicates if this payment was already executed and the amount already credited or charged to the respective
    ///     account.
    /// </summary>
    public bool IsCleared
    {
        get => isCleared;
        set => SetProperty( ref isCleared,   value);
    }

    /// <summary>
    ///     Type of the payment <see cref="PaymentType" />.
    /// </summary>
    public PaymentType Type
    {
        get => type;

        set
        {
            SetProperty(ref type, value);
            OnPropertyChanged(nameof(IsTransfer));
        }
    }

    /// <summary>
    ///     Additional notes to the payment.
    /// </summary>
    public string Note
    {
        get => note;
        set => SetProperty( ref note,   value);
    }

    /// <summary>
    ///     Indicates if the payment will be repeated or if it's a unique payment.
    /// </summary>
    public bool IsRecurring
    {
        get => isRecurring;

        set
        {
            SetProperty(ref isRecurring, value);
            RecurringPayment = isRecurring ? new RecurringPaymentViewModel() : null;
            OnPropertyChanged(nameof(RecurringPayment));
        }
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty( ref created,   value);
    }

    public DateTime LastModified
    {
        get => lastModified;
        set => SetProperty( ref lastModified,   value);
    }

    /// <summary>
    ///     In case it's a expense or transfer the account who will be charged.     In case it's an income the account
    ///     who will be credited.
    /// </summary>
    public AccountViewModel ChargedAccount
    {
        get => chargedAccount;
        set => SetProperty( ref chargedAccount,   value);
    }

    /// <summary>
    ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.     Not used for the other payment
    ///     types.
    /// </summary>
    public AccountViewModel? TargetAccount
    {
        get => targetAccount;
        set => SetProperty( ref targetAccount,   value);
    }

    /// <summary>
    ///     The <see cref="Category" /> for this payment
    /// </summary>
    public SelectedCategoryViewModel? Category
    {
        get => categoryViewModel;
        set => SetProperty( ref categoryViewModel,   value);
    }

    /// <summary>
    ///     The <see cref="RecurringPayment" /> if it's recurring.
    /// </summary>
    public RecurringPaymentViewModel? RecurringPayment
    {
        get => recurringPaymentViewModel;
        set => SetProperty( ref recurringPaymentViewModel,   value);
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
        set => SetProperty( ref currentAccountId,   value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, SelectedCategoryViewModel>();
        configuration.CreateMap<Payment, PaymentListItemViewModel>()
            .ForMember(destinationMember: x => x.CurrentAccountId, memberOptions: opt => opt.Ignore())
            .ReverseMap();
    }
}

