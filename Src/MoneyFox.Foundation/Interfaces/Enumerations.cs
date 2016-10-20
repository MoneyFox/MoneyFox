namespace MoneyFox.Foundation
{
    public enum PaymentType
    {
        Expense,
        Income,
        Transfer
    }

    public enum PaymentRecurrence
    {
        Daily = 0,
        DailyWithoutWeekend = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4,
        Biweekly = 5
    }

    public enum ListStatisticType
    {
        CategorySpreading,
        CategorySummary
    }

    public enum InvocationType
    {
        Account,
        Payment,
        Setting
    }

    public enum StatisticType
    {
        Cashflow,
        CategorySpreading,
        CategorySummary,
        ExpenseHistory
    }

    public enum TyleType
    {
        Income,
        Expense,
        Transfer
    }
}