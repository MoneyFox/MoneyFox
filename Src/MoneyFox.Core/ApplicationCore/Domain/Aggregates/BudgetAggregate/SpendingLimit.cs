namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate
{

    using Exceptions;

    public readonly struct SpendingLimit
    {
        public decimal Value { get; }

        public SpendingLimit(decimal val)
        {
            if (val < 1)
            {
                throw new InvalidArgumentException(nameof(val), "Value can't be 0 or negative.");
            }
            Value = val;
        }
    }

}
