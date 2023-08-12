namespace MoneyFox.Core.Queries.PaymentDataById;

using System;
using MoneyFox.Domain.Aggregates;
using MoneyFox.Domain.Aggregates.AccountAggregate;

public record PaymentData(
    int PaymentId,
    decimal Amount,
    int ChargedAccountId,
    int? TargetAccountId,
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

public record AccountData(int Id, string Name);
public record CategoryData(int Id, string Name, bool RequireNote);

public record RecurrenceData(Recurrence Recurrence, DateOnly StartDate, DateOnly? EndDate)
{
    public bool IsEndless => EndDate is null;
}
