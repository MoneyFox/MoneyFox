namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.CategoryAggregate;
using Domain.Aggregates.LedgerAggregate;

internal static class TestLedgerDbFactory
{
    internal static Ledger CreateDbLedger(this TestData.ILedger ledger)
    {
        return Ledger.Create(ledger.Name, ledger.CurrentBalance, ledger.Note, ledger.IsExcluded);
    }
}
