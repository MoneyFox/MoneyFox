namespace MoneyFox.Core.Common.Helpers;

using ApplicationCore.Domain.Aggregates.AccountAggregate;

public static class RecurringPaymentHelper
{
    public static bool AllowLastDayOfMonth(PaymentRecurrence passedEnum)
    {
        // TODO: Change to a switch expression using 'or' for the multiple condition matches when the project is upgraded to C# 9.
        return passedEnum switch
        {
            PaymentRecurrence.Monthly
                or PaymentRecurrence.Bimonthly
                or PaymentRecurrence.Quarterly
                or PaymentRecurrence.Biannually
                or PaymentRecurrence.Yearly => true,
            _ => false
        };
    }
}
