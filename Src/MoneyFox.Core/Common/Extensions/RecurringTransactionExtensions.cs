namespace MoneyFox.Core.Common.Extensions;

using System;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;

public static class RecurringTransactionExtensions
{
    public static Recurrence ToRecurrence(this PaymentRecurrence recurrence)
    {
        return recurrence switch
        {
            PaymentRecurrence.Daily => Recurrence.Daily,
            PaymentRecurrence.DailyWithoutWeekend => Recurrence.DailyWithoutWeekend,
            PaymentRecurrence.Weekly => Recurrence.Weekly,
            PaymentRecurrence.Biweekly => Recurrence.Biweekly,
            PaymentRecurrence.Monthly => Recurrence.Monthly,
            PaymentRecurrence.Bimonthly => Recurrence.Bimonthly,
            PaymentRecurrence.Quarterly => Recurrence.Quarterly,
            PaymentRecurrence.Yearly => Recurrence.Quarterly,
            PaymentRecurrence.Biannually => Recurrence.Biannually,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(recurrence), actualValue: recurrence, message: null)
        };
    }
}
