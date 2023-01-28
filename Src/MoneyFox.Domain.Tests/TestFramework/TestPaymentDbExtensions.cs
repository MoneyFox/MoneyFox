namespace MoneyFox.Domain.Tests.TestFramework;

using MoneyFox.Domain.Aggregates.AccountAggregate;
using MoneyFox.Infrastructure.Persistence;

internal static class TestPaymentDbExtensions
{
    public static void RegisterPayments(this AppDbContext db, params TestData.IPayment[] payments)
    {
        foreach (var testPayment in payments)
        {
            db.Add(testPayment.CreateDbPayment());
        }

        db.SaveChanges();
    }

    public static Payment RegisterPayment(this AppDbContext db, TestData.IPayment testCategory)
    {
        var dbPayment = testCategory.CreateDbPayment();
        db.Add(dbPayment);
        db.SaveChanges();

        return dbPayment;
    }
}
