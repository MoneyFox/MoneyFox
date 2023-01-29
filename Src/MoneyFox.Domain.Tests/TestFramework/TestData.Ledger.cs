namespace MoneyFox.Domain.Tests.TestFramework;

using System.Collections.Immutable;
using Domain.Aggregates.LedgerAggregate;

internal static partial class TestData
{
    public sealed record SavingsLedger : ILedger
    {
        public LedgerId Id { get; } = new(2);
        public string Name { get; } = "Spendings";
        public Money CurrentBalance { get; } = new(amount: 1200, currency: Currencies.CHF);
        public string? Note { get; } = "Ledger for all the daily spending.";
        public bool IsExcluded { get; } = true;

        public IReadOnlyCollection<ILedger.ITransaction> Transactions { get; }
            = ImmutableList.Create<ILedger.ITransaction>(new BeverageTransaction(), new SalaryTransaction());

        public sealed record BeverageTransaction : ILedger.ITransaction
        {
            public TransactionId Id { get; } = new(210);
            public TransactionType Type { get; } = TransactionType.Expense;
            public Money Amount { get; } = new(amount: 42, currency: Currencies.CHF);
            public DateOnly BookingDate { get; } = new(year: 2022, month: 11, day: 13);
            public int? CategoryId { get; } = 1;
            public string? Note { get; } = "Beverages";
        }

        public sealed record SalaryTransaction : ILedger.ITransaction
        {
            public TransactionId Id { get; } = new(211);
            public TransactionType Type { get; } = TransactionType.Income;
            public Money Amount { get; } = new(amount: 5432, currency: Currencies.CHF);
            public DateOnly BookingDate { get; } = new(year: 2022, month: 11, day: 25);
            public int? CategoryId { get; } = 2;
            public string? Note { get; } = "Salary";
        }
    }

    public interface ILedger
    {
        LedgerId Id { get; }
        string Name { get; }
        Money CurrentBalance { get; }
        string? Note { get; }
        bool IsExcluded { get; }
        IReadOnlyCollection<ITransaction> Transactions { get; }

        public interface ITransaction
        {
            TransactionId Id { get; }
            TransactionType Type { get; }
            Money Amount { get; }
            DateOnly BookingDate { get; }
            int? CategoryId { get; }
            string? Note { get; }
        }
    }
}
