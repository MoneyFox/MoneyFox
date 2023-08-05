namespace MoneyFox.Core.Queries.PaymentDataById;

using System;
using MoneyFox.Domain.Aggregates;
using MoneyFox.Domain.Aggregates.AccountAggregate;

public record PaymentData(
    int PaymentId,
    decimal Amount,
    int ChargedAccountId,
    int? TargetAccountId,
    DateTime Date,
    bool IsCleared,
    PaymentType Type,
    string Note,
    DateTime Created,
    DateTime? LastModified,
    RecurrenceData? RecurrenceData);

public record RecurrenceData(Recurrence Recurrence, DateTime StartDate, DateTime? EndDate);
