namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.LedgerAggregate;
using Infrastructure.Persistence;

internal static class TestLedgerDbExtensions
{
    public static void RegisterLedgers(this AppDbContext db, params TestData.ILedger[] testLedgers)
    {
        foreach (var ledger in testLedgers)
        {
            db.Add(ledger.CreateDbLedger());
        }

        db.SaveChanges();
    }

    public static Ledger RegisterLedger(this AppDbContext db, TestData.ILedger testLedger)
    {
        var dbLedger = testLedger.CreateDbLedger();
        db.Add(dbLedger);
        db.SaveChanges();

        return dbLedger;
    }
}
