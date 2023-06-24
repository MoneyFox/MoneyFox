namespace MoneyFox.Domain.Tests.TestFramework;

using System.Collections.Immutable;
using Domain.Aggregates.LedgerAggregate;

internal static partial class TestData
{
    public sealed record SavingsLedger : ILedger
    {
        public LedgerId Id { get; } = new(2);
        public string Name { get; } = "Spendings";
        public Money OpeningBalance { get; init; } = new(amount: 1200, currency: Currencies.CHF);
        public string? Note { get; } = "Ledger for all the daily spending.";
        public bool IsExcludeFromEndOfMonthSummary { get; } = true;
    }

    public interface ILedger
    {
        LedgerId Id { get; }
        string Name { get; }
        Money OpeningBalance { get; }
        string? Note { get; }
        bool IsExcludeFromEndOfMonthSummary { get; }
    }
}
