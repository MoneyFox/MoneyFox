namespace MoneyFox.Domain.Aggregates.BudgetAggregate;

using Exceptions;

public sealed record BudgetInterval(int NumberOfMonths)
{
    public static implicit operator decimal(BudgetInterval budgetInterval)
    {
        return budgetInterval.NumberOfMonths;
    }
}
