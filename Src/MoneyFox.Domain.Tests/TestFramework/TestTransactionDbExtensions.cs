namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.LedgerAggregate;
using Infrastructure.Persistence;

internal static class TestTransactionDbExtensions
{
    public static Transaction RegisterTransaction(this AppDbContext db, TestData.ITransaction testTransaction)
    {
        var dbTransaction = testTransaction.CreateDbTransaction();
        db.Add(dbTransaction);
        db.SaveChanges();

        return dbTransaction;
    }

    public static void RegisterTransactions(this AppDbContext db, params TestData.ITransaction[] testTransactions)
    {
        foreach (var transaction in testTransactions)
        {
            db.Add(transaction.CreateDbTransaction());
        }

        db.SaveChanges();
    }
}
