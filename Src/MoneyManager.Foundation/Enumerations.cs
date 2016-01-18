namespace MoneyManager.Foundation
{
    public enum PaymentType
    {
        Spending,
        Income,
        Transfer
    }

    public enum TransactionRecurrence
    {
        Daily = 0,
        DailyWithoutWeekend = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4,
        Biweekly = 5,

    }

    public enum ListStatisticType
    {
        CategorySpreading,
        CategorySummary
    }

    public enum InvocationType
    {
        Account,
        Transaction,
        Setting
    }

    public enum TaskCompletionType
    {
        Successful,
        Unsuccessful,
        Aborted
    }

    public enum StatisticType
    {
        Cashflow,
        CategorySpreading
    }
}