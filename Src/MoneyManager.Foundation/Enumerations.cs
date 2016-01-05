namespace MoneyManager.Foundation
{
    public enum TransactionType
    {
        Spending,
        Income,
        Transfer
    }

    public enum TransactionRecurrence
    {
        Daily,
        DailyWithoutWeekend,
        Weekly,
        Biweekly,
        Monthly,
        Yearly
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