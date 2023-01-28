namespace MoneyFox.Core.ApplicationCore.Queries.GetPaymentsForAccountIdQuery;

using System;
using Domain.Aggregates.AccountAggregate;

public static class PaymentTypeFilterHelper
{
    public static PaymentType PaymentTypeFilterToPaymentType(PaymentTypeFilter paymentTypeFilter)
    {
        return paymentTypeFilter switch
        {
            PaymentTypeFilter.Expense => PaymentType.Expense,
            PaymentTypeFilter.Income => PaymentType.Income,
            PaymentTypeFilter.Transfer => PaymentType.Transfer,
            _ => throw new InvalidCastException()
        };
    }
}
