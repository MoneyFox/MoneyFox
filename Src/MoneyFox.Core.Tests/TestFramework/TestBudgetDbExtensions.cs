namespace MoneyFox.Core.Tests.TestFramework;

using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Infrastructure.Persistence;

internal static class TestBudgetDbExtensions
{
    public static void RegisterBudgets(this AppDbContext db, params TestData.IBudget[] budgets)
    {
        foreach (var testBudget in budgets)
        {
            db.Add(testBudget.CreateDbBudget());
        }

        db.SaveChanges();
    }

    public static Budget RegisterBudget(this AppDbContext db, TestData.IBudget budget)
    {
        var dbBudget = budget.CreateDbBudget();
        db.Add(dbBudget);
        db.SaveChanges();

        return dbBudget;
    }
}
