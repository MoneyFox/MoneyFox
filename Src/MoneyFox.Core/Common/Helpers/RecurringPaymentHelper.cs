namespace MoneyFox.Core.Common.Helpers
{

    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

    public static class RecurringPaymentHelper
    {
        public static bool AllowLastDayOfMonth(PaymentRecurrence passedEnum)
        {
            // TODO: Change to a switch expression using 'or' for the multiple condition matches when the project is upgraded to C# 9.
            switch (passedEnum)
            {
                case PaymentRecurrence.Monthly:
                case PaymentRecurrence.Bimonthly:
                case PaymentRecurrence.Quarterly:
                case PaymentRecurrence.Biannually:
                case PaymentRecurrence.Yearly:
                    return true;
                default:
                    return false;
            }
        }
    }

}
