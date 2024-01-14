namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.AccountAggregate;

public class PaymentListItemViewModel : ObservableObject
{
    private decimal amount;
    private string? categoryName;
    private int chargedAccountId;
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

    public int ChargedAccountId
    {
        get => chargedAccountId;
        set => SetProperty(field: ref chargedAccountId, newValue: value);
    }

    public DateTime Date
    {
        get => date;
        set => SetProperty(field: ref date, newValue: value);
    }

    public decimal Amount
    {
        get => amount;
        set => SetProperty(field: ref amount, newValue: value);
    }

    public bool IsCleared
    {
        get => isCleared;
        set => SetProperty(field: ref isCleared, newValue: value);
    }

    public PaymentType Type
    {
        get => type;

        set
        {
            SetProperty(field: ref type, newValue: value);
            OnPropertyChanged(nameof(IsTransfer));
        }
    }

    public string Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public bool IsRecurring
    {
        get => isRecurring;
        set => SetProperty(field: ref isRecurring, newValue: value);
    }

    public string? CategoryName
    {
        get => categoryName;
        set => SetProperty(field: ref categoryName, newValue: value);
    }

    public bool IsTransfer => Type == PaymentType.Transfer;
}
