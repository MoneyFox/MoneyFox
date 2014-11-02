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
        Monthly,
        Yearly
    }

    public enum InvocationType
    {
        Account,
        Transaction,
        Setting
    }
}