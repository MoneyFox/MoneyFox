namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.LedgerAggregate;

internal static partial class TestData
{
    public sealed record BeverageTransaction : ITransaction
    {
        public TransactionId Id { get; } = new(210);
        public Guid Reference { get; } = Guid.NewGuid();
        public TransactionType Type { get; } = TransactionType.Expense;
        public Money Amount { get; init; } = new(amount: -42, currency: Currencies.CHF);
        public Money LedgerBalance { get; } = new(amount: 102, currency: Currencies.CHF);
        public DateOnly BookingDate { get; } = new(year: 2022, month: 11, day: 13);
        public int? CategoryId { get; } = 1;
        public string? Note { get; } = "Beverages";
        public bool IsTransfer { get; } = false;
    }

    public sealed record SalaryTransaction : ITransaction
    {
        public TransactionId Id { get; } = new(211);
        public Guid Reference { get; } = Guid.NewGuid();
        public TransactionType Type { get; } = TransactionType.Income;
        public Money Amount { get; init; } = new(amount: 5432, currency: Currencies.CHF);
        public Money LedgerBalance { get; } = new(amount: 6231, currency: Currencies.CHF);
        public DateOnly BookingDate { get; } = new(year: 2022, month: 11, day: 25);
        public int? CategoryId { get; } = 2;
        public string? Note { get; } = "Salary";
        public bool IsTransfer { get; } = false;
    }

    public interface ITransaction
    {
        TransactionId Id { get; }
        Guid Reference { get; }
        TransactionType Type { get; }
        Money Amount { get; }
        Money LedgerBalance { get; }
        DateOnly BookingDate { get; }
        int? CategoryId { get; }
        string? Note { get; }
        bool IsTransfer { get; }
    }
}
