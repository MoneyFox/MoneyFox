namespace MoneyFox.Core.Common.Extensions;

using System;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.RecurringTransactionAggregate;

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

    public static PaymentRecurrence ToPaymentRecurrence(this Recurrence recurrence)
    {
        return recurrence switch
        {
            Recurrence.Daily => PaymentRecurrence.Daily,
            Recurrence.DailyWithoutWeekend => PaymentRecurrence.DailyWithoutWeekend,
            Recurrence.Weekly => PaymentRecurrence.Weekly,
            Recurrence.Biweekly => PaymentRecurrence.Biweekly,
            Recurrence.Monthly => PaymentRecurrence.Monthly,
            Recurrence.Bimonthly => PaymentRecurrence.Bimonthly,
            Recurrence.Quarterly => PaymentRecurrence.Quarterly,
            Recurrence.Yearly => PaymentRecurrence.Quarterly,
            Recurrence.Biannually => PaymentRecurrence.Biannually,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(recurrence), actualValue: recurrence, message: null)
        };
    }
}
