namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using Accounts.AccountModification;
using CommunityToolkit.Mvvm.ComponentModel;
using Controls.AccountPicker;
using Domain.Aggregates.AccountAggregate;

public class PaymentViewModel : ObservableObject
{
    private decimal amount;

    private AccountPickerViewModel chargedAccount = null!;
    private DateTime created;
    private DateTime date;
    private int id;
    private bool isCleared;
    private bool isRecurring;
    private DateTime? lastModified;
    private string note = "";
    private AccountPickerViewModel? targetAccount;
    private PaymentType type;

    public PaymentViewModel()
    {
        Date = DateTime.Today;
    }

    public int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    /// <summary>
    ///     Date when this payment will be executed.
    /// </summary>
    public DateTime Date
    {
        get => date;
        set => SetProperty(field: ref date, newValue: value);
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
    ///     Indicates if this payment was already executed and the amount already credited or charged to the respective
    ///     account.
    /// </summary>
    public bool IsCleared
    {
        get => isCleared;
        set => SetProperty(field: ref isCleared, newValue: value);
    }

    /// <summary>
    ///     Type of the payment <see cref="PaymentType" />.
    /// </summary>
    public PaymentType Type
    {
        get => type;

        set
        {
            SetProperty(field: ref type, newValue: value);
            OnPropertyChanged(nameof(IsTransfer));
        }
    }

    /// <summary>
    ///     Additional notes to the payment.
    /// </summary>
    public string? Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    /// <summary>
    ///     Indicates if the payment will be repeated or if it's a unique payment.
    /// </summary>
    public bool IsRecurring
    {
        get => isRecurring;
        set => SetProperty(field: ref isRecurring, newValue: value);
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty(field: ref created, newValue: value);
    }

    public DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(field: ref lastModified, newValue: value);
    }

    /// <summary>
    ///     In case it's a expense or transfer the account who will be charged.     In case it's an income the account
    ///     who will be credited.
    /// </summary>
    public AccountPickerViewModel ChargedAccount
    {
        get => chargedAccount;
        set => SetProperty(field: ref chargedAccount, newValue: value);
    }

    /// <summary>
    ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.     Not used for the other payment
    ///     types.
    /// </summary>
    public AccountPickerViewModel? TargetAccount
    {
        get => targetAccount;
        set => SetProperty(field: ref targetAccount, newValue: value);
    }

    /// <summary>
    ///     This is a shortcut to access if the payment is a transfer or not.
    /// </summary>
    public bool IsTransfer => Type == PaymentType.Transfer;
}
