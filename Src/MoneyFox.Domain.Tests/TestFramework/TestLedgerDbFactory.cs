namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.LedgerAggregate;

internal static class TestLedgerDbFactory
{
    internal static Ledger CreateDbLedger(this TestData.ILedger ledger)
    {
        return Ledger.Create(name: ledger.Name, currentBalance: ledger.CurrentBalance, note: ledger.Note, isExcluded: ledger.IsExcludeFromEndOfMonthSummary);
    }
}
