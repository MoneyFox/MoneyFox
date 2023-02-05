namespace MoneyFox.Domain.Aggregates.BudgetAggregate;

public sealed record BudgetInterval(int NumberOfMonths)
{
    public static implicit operator decimal(BudgetInterval budgetInterval)
    {
        return budgetInterval.NumberOfMonths;
    }
}
