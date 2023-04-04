﻿namespace MoneyFox.Domain.Tests.TestFramework;

using System.Collections.Immutable;
using Domain.Aggregates.LedgerAggregate;

internal static partial class TestData
{
    public sealed record SavingsLedger : ILedger
    {
        public LedgerId Id { get; } = new(2);
        public string Name { get; } = "Spendings";
        public Money CurrentBalance { get; init; } = new(amount: 1200, currency: Currencies.CHF);
        public string? Note { get; } = "Ledger for all the daily spending.";
        public bool IsExcludeFromEndOfMonthSummary { get; } = true;

        public IReadOnlyCollection<ILedger.ITransaction> Transactions { get; init; }
            = ImmutableList.Create<ILedger.ITransaction>(new BeverageTransaction(), new SalaryTransaction());

        public sealed record BeverageTransaction : ILedger.ITransaction
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

        public sealed record SalaryTransaction : ILedger.ITransaction
        {
            public TransactionId Id { get; } = new(211);
            public Guid Reference { get; } = Guid.NewGuid();
            public TransactionType Type { get; } = TransactionType.Income;
            public Money Amount { get; init;  } = new(amount: 5432, currency: Currencies.CHF);
            public Money LedgerBalance { get; } = new(amount: 6231, currency: Currencies.CHF);
            public DateOnly BookingDate { get; } = new(year: 2022, month: 11, day: 25);
            public int? CategoryId { get; } = 2;
            public string? Note { get; } = "Salary";
            public bool IsTransfer { get; } = false;
        }
    }

    public interface ILedger
    {
        LedgerId Id { get; }
        string Name { get; }
        Money CurrentBalance { get; }
        string? Note { get; }
        bool IsExcludeFromEndOfMonthSummary { get; }
        IReadOnlyCollection<ITransaction> Transactions { get; }

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
}