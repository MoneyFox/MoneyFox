namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.LedgerAggregate;

internal static partial class TestData
{
    public sealed class SpendingLedger : ILedger
    {
        public LedgerId Id { get; } = new(12);
        public string Name { get; } = "Spendings";
        public Money CurrentBalance { get; } = new Money(1200, Currencies.CHF);
        public string? Note { get; } = "Ledger for all the daily spending.";
        public bool IsExcluded { get; } = false;
    }

    public interface ILedger
    {
        LedgerId Id { get; }
        string Name { get; }
        Money CurrentBalance { get; }
        string? Note { get; }
        bool IsExcluded { get; }
    }

}
