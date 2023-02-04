namespace MoneyFox.Domain.Aggregates.BudgetAggregate;

using Exceptions;

public sealed record BudgetInterval
{
    public BudgetInterval(int numberOfMonths)
    {
        if (numberOfMonths < 1)
        {
            throw new InvalidArgumentException(paramName: nameof(numberOfMonths), message: "Value can't be 0 or negative.");
        }

        NumberOfMonths = numberOfMonths;
    }

    public int NumberOfMonths { get; }

    public static implicit operator decimal(BudgetInterval budgetInterval)
    {
        return budgetInterval.NumberOfMonths;
    }
}
