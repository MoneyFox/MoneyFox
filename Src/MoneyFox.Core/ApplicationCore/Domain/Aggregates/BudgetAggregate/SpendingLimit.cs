namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

using Exceptions;

public sealed record SpendingLimit
{
    public SpendingLimit(decimal value)
    {
        if (value < 1)
        {
            throw new InvalidArgumentException(paramName: nameof(value), message: "Value can't be 0 or negative.");
        }

        Value = value;
    }

    public decimal Value { get; }

    public static implicit operator decimal(SpendingLimit spendingLimit)
    {
        return spendingLimit.Value;
    }
}
