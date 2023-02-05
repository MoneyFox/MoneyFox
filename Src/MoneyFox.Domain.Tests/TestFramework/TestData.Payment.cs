namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.AccountAggregate;

internal static partial class TestData
{
    private static readonly DateTime defaultDate = new(year: 2022, month: 04, day: 06);

    internal sealed record DefaultExpense : IPayment
    {
        public int Id { get; set; } = 10;
        public IAccount ChargedAccount => new DefaultAccount();
        public IAccount? TargetAccount => null;
        public ICategory Category { get; init; } = new ExpenseCategory();
        public DateTime Date { get; init; } = defaultDate;
        public decimal Amount { get; init; } = 105.50m;
        public bool IsCleared => true;
        public PaymentType Type { get; init; } = PaymentType.Expense;
        public bool IsRecurring => false;
        public string Note => "6 Bottles";

        internal sealed record ExpenseCategory : ICategory
        {
            public int Id { get; set; }
            public string Name { get; } = "Whine";
            public string? Note { get; } = "Yummi!";
            public bool RequireNote { get; } = false;
        }
    }

    internal sealed record DefaultIncome : IPayment
    {
        public int Id { get; set; } = 10;
        public IAccount ChargedAccount => new DefaultAccount();
        public IAccount? TargetAccount => null;
        public ICategory? Category { get; init; } = new IncomeCategory();
        public DateTime Date { get; } = defaultDate;
        public decimal Amount => 105.50m;
        public bool IsCleared => true;
        public PaymentType Type { get; set; } = PaymentType.Income;
        public string Note => string.Empty;
        public bool IsRecurring => false;

        internal sealed record IncomeCategory : ICategory
        {
            public int Id { get; set; }
            public string Name { get; } = "Salary";
            public string? Note { get; } = "Yey!";
            public bool RequireNote { get; } = false;
        }
    }

    public sealed record DefaultAccount : IAccount
    {
        public int Id { get; } = 10;
        public string Name { get; } = "Spending";
        public decimal CurrentBalance { get; } = 890.60m;
        public string Note => string.Empty;
        public bool IsExcluded => false;
        public bool IsDeactivated => false;
    }

    internal interface IPayment
    {
        int Id { get; }

        IAccount ChargedAccount { get; }

        IAccount? TargetAccount { get; }

        ICategory? Category { get; init; }

        DateTime Date { get; }

        decimal Amount { get; }

        bool IsCleared { get; }

        PaymentType Type { get; }

        string Note { get; }

        bool IsRecurring { get; }
    }

    internal interface IAccount
    {
        int Id { get; }

        string Name { get; }

        decimal CurrentBalance { get; }

        string Note { get; }

        bool IsExcluded { get; }

        bool IsDeactivated { get; }
    }
}
