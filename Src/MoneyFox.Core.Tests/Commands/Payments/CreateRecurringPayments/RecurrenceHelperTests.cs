namespace MoneyFox.Core.Tests.Commands.Payments.CreateRecurringPayments;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Payments.CreateRecurringPayments;
using FluentAssertions;

[ExcludeFromCodeCoverage]
public class RecurrenceHelperTests
{
    public static IEnumerable<object[]> CheckIfRepeatableTestData()
    {
        // True - General conditions
        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Daily,
                PaymentDate = DateTime.Today.AddDays(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Weekly,
                PaymentDate = DateTime.Today.AddDays(-7),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Biweekly,
                PaymentDate = DateTime.Today.AddDays(-14),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Monthly,
                PaymentDate = DateTime.Today.AddMonths(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromPriorMonth(1)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Bimonthly,
                PaymentDate = DateTime.Today.AddMonths(-2),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromPriorMonth(2)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Quarterly,
                PaymentDate = DateTime.Today.AddMonths(-3),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromPriorMonth(3)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Biannually,
                PaymentDate = DateTime.Today.AddMonths(-6),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromPriorMonth(6)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Yearly,
                PaymentDate = DateTime.Today.AddMonths(-12),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromPriorMonth(12)
            }
        };

        // True - Boundary conditions
        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Monthly,
                PaymentDate = DateTime.Today.GetFirstDayOfMonth().AddDays(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromLastDayOfMonth(1)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Bimonthly,
                PaymentDate = DateTime.Today.AddMonths(-1).GetFirstDayOfMonth().AddDays(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromLastDayOfMonth(2)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Quarterly,
                PaymentDate = DateTime.Today.AddMonths(-2).GetFirstDayOfMonth().AddDays(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromLastDayOfMonth(3)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Biannually,
                PaymentDate = DateTime.Today.AddMonths(-5).GetFirstDayOfMonth().AddDays(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromLastDayOfMonth(6)
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Yearly,
                PaymentDate = DateTime.Today.AddMonths(-11).GetFirstDayOfMonth().AddDays(-1),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = RecurringPaymentTestData.GetSameDayFromLastDayOfMonth(12)
            }
        };

        // False - Boundary conditions
        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Daily,
                PaymentDate = DateTime.Today,
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Weekly,
                PaymentDate = DateTime.Today.AddDays(-6),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Biweekly,
                PaymentDate = DateTime.Today.AddDays(-13),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Monthly,
                PaymentDate = DateTime.Today.GetFirstDayOfMonth(),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today.GetFirstDayOfMonth()
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Bimonthly,
                PaymentDate = DateTime.Today.AddMonths(-1).GetFirstDayOfMonth(),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today.GetFirstDayOfMonth()
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Quarterly,
                PaymentDate = DateTime.Today.AddMonths(-2).GetFirstDayOfMonth(),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today.GetFirstDayOfMonth()
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Biannually,
                PaymentDate = DateTime.Today.AddMonths(-5).GetFirstDayOfMonth(),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today.GetFirstDayOfMonth()
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Yearly,
                PaymentDate = DateTime.Today.AddMonths(-11).GetFirstDayOfMonth(),
                ExpectedRecurrenceResult = false,
                ExpectedNewPaymentDate = DateTime.Today.GetFirstDayOfMonth()
            }
        };

        // True - Last day of month conditions
        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Monthly,
                PaymentDate = DateTime.Today.GetFirstDayOfMonth().AddDays(-10),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today.GetLastDayOfMonth(),
                IsLastDayOfMonth = true
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Bimonthly,
                PaymentDate = DateTime.Today.AddMonths(-1).GetFirstDayOfMonth().AddDays(-10),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today.GetLastDayOfMonth(),
                IsLastDayOfMonth = true
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Quarterly,
                PaymentDate = DateTime.Today.AddMonths(-2).GetFirstDayOfMonth().AddDays(-10),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today.GetLastDayOfMonth(),
                IsLastDayOfMonth = true
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Biannually,
                PaymentDate = DateTime.Today.AddMonths(-5).GetFirstDayOfMonth().AddDays(-10),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today.GetLastDayOfMonth(),
                IsLastDayOfMonth = true
            }
        };

        yield return new[]
        {
            new RecurringPaymentTestData
            {
                RecurrenceType = PaymentRecurrence.Yearly,
                PaymentDate = DateTime.Today.AddMonths(-11).GetFirstDayOfMonth().AddDays(-10),
                ExpectedRecurrenceResult = true,
                ExpectedNewPaymentDate = DateTime.Today.GetLastDayOfMonth(),
                IsLastDayOfMonth = true
            }
        };
    }

    [Theory]
    [MemberData(nameof(CheckIfRepeatableTestData))]
    public void CheckIfRepeatable_ValidatedRecurrence(RecurringPaymentTestData recurringPaymentTestData)
    {
        var account = new Account("foo");
        var payment = new Payment(date: recurringPaymentTestData.PaymentDate, amount: 105, type: PaymentType.Expense, chargedAccount: account);
        payment.AddRecurringPayment(
            recurrence: recurringPaymentTestData.RecurrenceType,
            isLastDayOfMonth: recurringPaymentTestData.IsLastDayOfMonth,
            endDate: DateTime.Today);

        RecurrenceHelper.CheckIfRepeatable(payment).Should().Be(recurringPaymentTestData.ExpectedRecurrenceResult);
    }

    [Theory]
    [InlineData(PaymentRecurrence.Daily, 0)]
    [InlineData(PaymentRecurrence.Weekly, 5)]
    [InlineData(PaymentRecurrence.Biweekly, 10)]
    [InlineData(PaymentRecurrence.Monthly, 28)]
    [InlineData(PaymentRecurrence.Bimonthly, 55)]
    [InlineData(PaymentRecurrence.Quarterly, 88)]
    [InlineData(PaymentRecurrence.Biannually, 180)]
    [InlineData(PaymentRecurrence.Yearly, 340)]
    public void CheckIfRepeatable_UnclearedPayment_ReturnFalse(PaymentRecurrence recurrence, int amountOfDaysUntilRepeat)
    {
        var account = new Account("foo");
        var payment = new Payment(date: DateTime.Today.AddDays(amountOfDaysUntilRepeat), amount: 105, type: PaymentType.Expense, chargedAccount: account);
        payment.AddRecurringPayment(recurrence: recurrence, isLastDayOfMonth: false, endDate: DateTime.Today);
        RecurrenceHelper.CheckIfRepeatable(payment).Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(CheckIfRepeatableTestData))]
    public void CheckGetPaymentDateFromRecurring(RecurringPaymentTestData recurringPaymentTestData)
    {
        var account = new Account("foo");
        var payment = new Payment(date: recurringPaymentTestData.PaymentDate, amount: 105, type: PaymentType.Expense, chargedAccount: account);
        payment.AddRecurringPayment(
            recurrence: recurringPaymentTestData.RecurrenceType,
            isLastDayOfMonth: recurringPaymentTestData.IsLastDayOfMonth,
            endDate: DateTime.Today);

        RecurrenceHelper.GetPaymentDateFromRecurring(payment.RecurringPayment).Should().Be(recurringPaymentTestData.ExpectedNewPaymentDate);
    }

    public class RecurringPaymentTestData
    {
        public PaymentRecurrence RecurrenceType { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool ExpectedRecurrenceResult { get; set; }
        public DateTime ExpectedNewPaymentDate { get; set; }
        public bool IsLastDayOfMonth { get; set; }

        // Month traversal important for instances where the current day of month exceeds the number of days in the prior month
        public static DateTime GetSameDayFromPriorMonth(int monthsBack)
        {
            return DateTime.Today.AddMonths(-monthsBack).AddMonths(monthsBack);
        }

        public static DateTime GetSameDayFromLastDayOfMonth(int monthsBack)
        {
            return DateTime.Today.AddMonths(-monthsBack).GetLastDayOfMonth().AddMonths(monthsBack);
        }
    }
}
