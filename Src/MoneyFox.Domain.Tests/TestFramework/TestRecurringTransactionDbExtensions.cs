namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.RecurringTransactionAggregate;
using Infrastructure.Persistence;

internal static class TestRecurringTransactionDbExtensions
{
    public static void RegisterBudgets(this AppDbContext db, params TestData.IRecurringTransaction[] recurringTransactions)
    {
        foreach (var testRecurringTransaction in recurringTransactions)
        {
            db.Add(testRecurringTransaction.CreateDbRecurringTransaction());
        }

        db.SaveChanges();
    }

    public static RecurringTransaction RegisterBudget(this AppDbContext db, TestData.IRecurringTransaction recurringTransaction)
    {
        var dbRecurringTransaction = recurringTransaction.CreateDbRecurringTransaction();
        db.Add(dbRecurringTransaction);
        db.SaveChanges();
        recurringTransaction.Id = dbRecurringTransaction.Id;

        return dbRecurringTransaction;
    }
}
