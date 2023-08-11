namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.AccountAggregate;

internal static class TestPaymentDbFactory
{
    internal static Payment CreateDbPayment(this TestData.IPayment payment)
    {
        var dbChargedAccount = payment.ChargedAccount.CreateDbAccount();
        var dbTargetAccount = payment.TargetAccount?.CreateDbAccount();
        var dbCategory = payment.Category?.CreateDbCategory();

        //TODO Handle RecurringPayment better

        return new(
            date: payment.Date,
            amount: payment.Amount,
            type: payment.Type,
            chargedAccount: dbChargedAccount,
            targetAccount: dbTargetAccount,
            category: dbCategory,
            note: payment.Note,
            recurringTransactionId: payment.RecurringTransactionId);
    }
}
