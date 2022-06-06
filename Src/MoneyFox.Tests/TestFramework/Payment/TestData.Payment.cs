namespace MoneyFox.Tests.TestFramework.Payment
{

    using System;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

    internal sealed class TestData
    {
        internal static DateTime DefaultDate = new DateTime(year: 2022, month: 06, day: 06);

        internal sealed class DefaultExpense : IPayment
        {
            public int Id => 10;
            public IAccount ChargedAccount => new DefaultAccount();
            public IAccount? TargetAccount => null;
            public string CategoryName => "Wine";
            public DateTime Date => DefaultDate;
            public decimal Amount => 105.50m;
            public bool IsCleared { get; }
            public PaymentType Type { get; } = PaymentType.Expense;
            public string? Note { get; }
            public bool IsRecurring { get; }
        }

        public sealed class DefaultAccount : IAccount
        {
            public int Id { get; } = 10;
            public string Name { get; } = "Spendings";
            public decimal CurrentBalance { get; } = 890.60m;
            public string? Note { get; }
            public bool IsExcluded { get; }
            public bool IsDeactivated { get; }
        }

        internal interface IPayment
        {
            int Id { get; }

            IAccount ChargedAccount { get; }

            IAccount? TargetAccount { get; }

            string CategoryName { get; }

            DateTime Date { get; }

            decimal Amount { get; }

            bool IsCleared { get; }

            PaymentType Type { get; }

            string? Note { get; }

            bool IsRecurring { get; }
        }

        internal interface IAccount
        {
            int Id { get; }

            string Name { get; }

            decimal CurrentBalance { get; }

            string? Note { get; }

            bool IsExcluded { get; }

            bool IsDeactivated { get; }
        }
    }

}
