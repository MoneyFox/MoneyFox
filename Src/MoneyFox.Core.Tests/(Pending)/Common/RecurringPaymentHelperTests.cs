namespace MoneyFox.Core.Tests._Pending_.Common
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core._Pending_.Common.Extensions;
    using Core._Pending_.Common.Helpers;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using FluentAssertions;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class RecurringPaymentHelperTests
    {
        [Theory]
        [InlineData(PaymentRecurrence.Daily, 1, true)]
        [InlineData(PaymentRecurrence.Weekly, 8, true)]
        [InlineData(PaymentRecurrence.Biweekly, 14, true)]
        [InlineData(PaymentRecurrence.Monthly, 31, true)]
        [InlineData(PaymentRecurrence.Quarterly, 94, true)]
        [InlineData(PaymentRecurrence.Biannually, 184, true)]
        [InlineData(PaymentRecurrence.Yearly, 366, true)]
        [InlineData(PaymentRecurrence.Daily, 0, false)]
        [InlineData(PaymentRecurrence.Weekly, 5, false)]
        [InlineData(PaymentRecurrence.Biweekly, 10, false)]
        [InlineData(PaymentRecurrence.Bimonthly, 20, false)]
        [InlineData(PaymentRecurrence.Quarterly, 59, false)]
        [InlineData(PaymentRecurrence.Biannually, 137, false)]
        [InlineData(PaymentRecurrence.Yearly, 300, false)]
        [InlineData(PaymentRecurrence.Biannually, 355, true)] // with year change
        [InlineData(PaymentRecurrence.Quarterly, 355, true)] // with year change
        public void CheckIfRepeatable_ValidatedRecurrence(PaymentRecurrence recurrence, int amountOfDaysPassed, bool expectedResult)
        {
            var account = new Account("foo");
            var payment = new Payment(date: DateTime.Today.AddDays(-amountOfDaysPassed), amount: 105, type: PaymentType.Expense, chargedAccount: account);
            payment.AddRecurringPayment(recurrence: recurrence, endDate: DateTime.Today);
            RecurringPaymentHelper.CheckIfRepeatable(payment).Should().Be(expectedResult);
        }

        [Fact]
        public void CheckIfRepeatable_ValidatedRecurrence_Bimonthly()
        {
            var account = new Account("foo");
            var payment = new Payment(date: DateTime.Today.AddMonths(-2), amount: 105, type: PaymentType.Expense, chargedAccount: account);
            payment.AddRecurringPayment(recurrence: PaymentRecurrence.Bimonthly, endDate: DateTime.Today);
            RecurringPaymentHelper.CheckIfRepeatable(payment).Should().Be(true);
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily, 0)]
        [InlineData(PaymentRecurrence.Weekly, 5)]
        [InlineData(PaymentRecurrence.Biweekly, 10)]
        [InlineData(PaymentRecurrence.Monthly, 28)]
        [InlineData(PaymentRecurrence.Bimonthly, 55)]
        [InlineData(PaymentRecurrence.Yearly, 340)]
        public void CheckIfRepeatable_UnclearedPayment_ReturnFalse(PaymentRecurrence recurrence, int amountOfDaysUntilRepeat)
        {
            var account = new Account("foo");
            var payment = new Payment(date: DateTime.Today.AddDays(amountOfDaysUntilRepeat), amount: 105, type: PaymentType.Expense, chargedAccount: account);
            payment.AddRecurringPayment(recurrence: recurrence, endDate: DateTime.Today);
            RecurringPaymentHelper.CheckIfRepeatable(payment).Should().BeFalse();
        }

        [Fact]
        public void CheckIfRepeatable_ValidatedRecurrenceMonthly_False()
        {
            var account = new Account("foo");
            var payment = new Payment(date: DateTime.Today.GetFirstDayOfMonth(), amount: 105, type: PaymentType.Expense, chargedAccount: account);
            payment.AddRecurringPayment(recurrence: PaymentRecurrence.Monthly, endDate: DateTime.Today);
            RecurringPaymentHelper.CheckIfRepeatable(payment).Should().BeFalse();
        }
    }

}
