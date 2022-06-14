namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate
{

    using System.Collections.Generic;
    using Exceptions;

    public sealed class SpendingLimit : ValueObject
    {
        public decimal Value { get; }

        public SpendingLimit(decimal value)
        {
            if (value < 1)
            {
                throw new InvalidArgumentException(nameof(value), "Value can't be 0 or negative.");
            }
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator decimal(SpendingLimit spendingLimit)
        {
            return spendingLimit.Value;
        }
    }

}
