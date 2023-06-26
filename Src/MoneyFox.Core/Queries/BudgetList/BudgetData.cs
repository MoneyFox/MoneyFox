namespace MoneyFox.Core.Queries.BudgetList;

public record BudgetData(int Id,
    string Name,
    decimal SpendingLimit,
    decimal CurrentSpending,
    decimal MonthlyBudget,
    decimal MonthlySpending);
