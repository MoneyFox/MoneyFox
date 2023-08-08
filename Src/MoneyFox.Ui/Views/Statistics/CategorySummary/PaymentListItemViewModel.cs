namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using Accounts.AccountModification;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Controls.CategorySelection;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;

public class PaymentListItemViewModel : ObservableObject, IHaveCustomMapping
{
    private decimal amount;
    private SelectedCategoryViewModel? categoryViewModel;
    private int chargedAccountId;

    private int currentAccountId;
    private DateTime date;

    private int id;
    private bool isCleared;
    private bool isRecurring;
    private string note = "";
    private PaymentType type;

    public PaymentListItemViewModel()
    {
        Date = DateTime.Today;
    }

    public int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    /// <summary>
    ///     In case it's a expense or transfer the foreign key to the <see cref="AccountViewModel" /> who will be
    ///     charged.     In case it's an income the  foreign key to the <see cref="AccountViewModel" /> who will be
    ///     credited.
    /// </summary>
    public int ChargedAccountId
    {
        get => chargedAccountId;
        set => SetProperty(field: ref chargedAccountId, newValue: value);
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
    public string Note
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

    /// <summary>
    ///     The <see cref="Category" /> for this payment
    /// </summary>
    public SelectedCategoryViewModel? Category
    {
        get => categoryViewModel;
        set => SetProperty(field: ref categoryViewModel, newValue: value);
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
        set => SetProperty(field: ref currentAccountId, newValue: value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, SelectedCategoryViewModel>();
        configuration.CreateMap<Payment, PaymentListItemViewModel>()
            .ForMember(destinationMember: x => x.CurrentAccountId, memberOptions: opt => opt.Ignore())
            .ReverseMap();
    }
}
