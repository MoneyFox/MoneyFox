namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using CommunityToolkit.Mvvm.ComponentModel;
using Domain;

public class BudgetPaymentViewModel : ObservableObject
{
    private string accountName = null!;

    private Money amount = null!;

    private string category = null!;

    private bool isCleared;

    private bool isRecurring;

    public int Id { get; init; }

    public required string AccountName
    {
        get => accountName;
        set => SetProperty(field: ref accountName, newValue: value);
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
