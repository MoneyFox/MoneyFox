namespace MoneyFox.Core._Pending_.Common.Helpers
{

    using System;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;

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

}
