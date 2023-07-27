namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.RecurringTransactionAggregate;

internal static class TestRecurringTransactionDbExtensions
{
    internal static RecurringTransaction CreateDbLedger(this TestData.IRecurringTransaction recurringTransaction)
    {
        return RecurringTransaction.Create(
            id: recurringTransaction.Id,
            startDate: recurringTransaction.StartDate,
            endDate: recurringTransaction.EndDate,
            amount: recurringTransaction.Amount,
            type: recurringTransaction.Type,
            note: recurringTransaction.Note,
            chargedAccount: recurringTransaction.ChargedAccount,
            targetAccount: recurringTransaction.TargetAccount,
            categoryId: recurringTransaction.CategoryId,
            recurrence: recurringTransaction.Recurrence,
            isLastDayOfMonth: recurringTransaction.IsLastDayOfMonth);
    }
}
