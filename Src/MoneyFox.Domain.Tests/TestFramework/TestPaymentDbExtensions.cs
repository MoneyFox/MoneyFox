namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.AccountAggregate;
using Infrastructure.Persistence;

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

    public static Payment RegisterPayment(this AppDbContext db, TestData.IPayment testPayment)
    {
        var dbPayment = testPayment.CreateDbPayment();
        db.Add(dbPayment);
        db.SaveChanges();
        testPayment.Id = dbPayment.Id;
        testPayment.ChargedAccount.Id = dbPayment.ChargedAccount.Id;
        if (testPayment.Category is not null)
        {
            testPayment.Category.Id = dbPayment.Category!.Id;
        }

        if (testPayment.TargetAccount is not null && dbPayment.TargetAccount is not null)
        {
            testPayment.TargetAccount.Id = dbPayment.TargetAccount.Id;
        }

        return dbPayment;
    }
}
