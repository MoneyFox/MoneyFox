﻿namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.RecurringTransactionAggregate;

internal static class TestRecurringTransactionDbFactory
{
    internal static RecurringTransaction CreateDbRecurringTransaction(this TestData.IRecurringTransaction recurringTransaction)
    {
        return RecurringTransaction.Create(
            recurringTransactionId: recurringTransaction.RecurringTransactionId,
            chargedAccount: recurringTransaction.ChargedAccount,
            targetAccount: recurringTransaction.TargetAccount,
            amount: recurringTransaction.Amount,
            categoryId: recurringTransaction.CategoryId,
            startDate: recurringTransaction.StartDate,
            endDate: recurringTransaction.EndDate,
            recurrence: recurringTransaction.Recurrence,
            note: recurringTransaction.Note,
            isLastDayOfMonth: recurringTransaction.IsLastDayOfMonth,
            isTransfer: recurringTransaction.IsTransfer);
    }
}
