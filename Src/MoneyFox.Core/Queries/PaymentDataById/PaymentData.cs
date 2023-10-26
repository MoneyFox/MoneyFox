namespace MoneyFox.Core.Queries.PaymentDataById;

using System;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.RecurringTransactionAggregate;

public record PaymentData(
    int PaymentId,
    decimal Amount,
    AccountData ChargedAccount,
    AccountData? TargetAccount,
    CategoryData? Category,
    DateTime Date,
    bool IsCleared,
    PaymentType Type,
    string Note,
    DateTime Created,
    DateTime? LastModified,
    RecurrenceData? RecurrenceData)
{
    public bool IsRecurring => RecurrenceData is not null;
}

public record AccountData(int Id, string Name, decimal CurrentBalance);
public record CategoryData(int Id, string Name, bool RequireNote);

public record RecurrenceData(Recurrence Recurrence, DateOnly StartDate, DateOnly? EndDate)
{
    public bool IsEndless => EndDate is null;
}
