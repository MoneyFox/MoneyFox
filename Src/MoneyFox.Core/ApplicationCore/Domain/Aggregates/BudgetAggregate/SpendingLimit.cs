namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

using System.Collections.Generic;
using Exceptions;

public sealed class SpendingLimit : ValueObject
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator decimal(SpendingLimit spendingLimit) => spendingLimit.Value;
}

