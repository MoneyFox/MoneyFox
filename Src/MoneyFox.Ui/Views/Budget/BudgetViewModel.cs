namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.ComponentModel;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

public sealed class BudgetViewModel : ObservableObject
{
    private string name = null!;

    private decimal spendingLimit;
    private BudgetTimeRange timeRange;
    public int Id { get; set; }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public BudgetTimeRange TimeRange
    {
        get => timeRange;
        set => SetProperty(field: ref timeRange, newValue: value);
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;
        set => SetProperty(field: ref spendingLimit, newValue: value);
    }
}

