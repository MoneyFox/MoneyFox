namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using CommunityToolkit.Mvvm.ComponentModel;
using Domain;

public class BudgetPaymentViewModel : ObservableObject
{
    private string account = string.Empty;
    private Money amount = Money.Zero("USD");
    private string category = string.Empty;
    private string note = string.Empty;
    private DateOnly date;
    private bool isCleared;
    private bool isRecurring;

    public int Id { get; init; }

    public required string Account
    {
        get => account;
        set => SetProperty(field: ref account, newValue: value);
    }

    public required DateOnly Date
    {
        get => date;
        set => SetProperty(field: ref date, newValue: value);
    }

    public required Money Amount
    {
        get => amount;
        set => SetProperty(field: ref amount, newValue: value);
    }

    public required string Category
    {
        get => category;
        set => SetProperty(field: ref category, newValue: value);
    }

    public required string Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public required bool IsCleared
    {
        get => isCleared;
        set => SetProperty(field: ref isCleared, newValue: value);
    }

    public required bool IsRecurring
    {
        get => isRecurring;
        set => SetProperty(field: ref isRecurring, newValue: value);
    }
}
