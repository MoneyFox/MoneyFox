namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.RecurringTransactionAggregate;

internal static class TestRecurringTransactionDbFactory
{
    internal static RecurringTransaction CreateDbRecurringTransaction(this TestData.IRecurringTransaction recurringTransaction)
    {
        return RecurringTransaction.Create(
            id: recurringTransaction.Id,
            chargedAccount: recurringTransaction.ChargedAccount,
            targetAccount: recurringTransaction.TargetAccount,
            amount: recurringTransaction.Amount,
            categoryId: recurringTransaction.CategoryId,
            type: recurringTransaction.Type,
            startDate: recurringTransaction.StartDate,
            endDate: recurringTransaction.EndDate,
            recurrence: recurringTransaction.Recurrence,
            note: recurringTransaction.Note,
            isLastDayOfMonth: recurringTransaction.IsLastDayOfMonth);
    }
}
