namespace MoneyFox.Ui.Views.Payments;

using Domain.Aggregates.AccountAggregate;

public static class RecurringPaymentHelper
{
    public static bool AllowLastDayOfMonth(PaymentRecurrence passedEnum)
    {
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
