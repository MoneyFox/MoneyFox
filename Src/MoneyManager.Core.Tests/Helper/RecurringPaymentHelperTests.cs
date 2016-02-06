using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Xunit;
using XunitShouldExtension;
using Assert = Xunit.Assert;

namespace MoneyManager.Core.Tests.Helper
{
    public class RecurringPaymentHelperTests
    {
        [Theory]
        [InlineData(0, "7.8.2016", true, 1)]
        [InlineData(1, "7.8.2016", true, 2)]
        [InlineData(2, "7.8.2016", false, 3)]
        [InlineData(3, "7.8.2016", true, 1)]
        [InlineData(4, "7.8.2016", false, 3)]
        public void GetRecurringFromPayment(int recurrence, string date, bool isEndless, int type)
        {
            var startDate = new DateTime(2015, 03, 12);
            var enddate = Convert.ToDateTime(date);

            var payment = new Payment
            {
                ChargedAccount = new Account {Id = 3},
                TargetAccount = new Account {Id = 8},
                Category = new Category {Id = 16},
                Date = startDate,
                Amount = 2135,
                IsCleared = false,
                Type = type,
                IsRecurring = true
            };

            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, isEndless,
                recurrence, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(isEndless);
            recurring.Amount.ShouldBe(payment.Amount);
            recurring.Category.Id.ShouldBe(payment.Category.Id);
            recurring.Type.ShouldBe(type);
            recurring.Recurrence.ShouldBe(recurrence);
            recurring.Note.ShouldBe(payment.Note);
        }

        [Theory]
        [InlineData(0, "Expense")]
        [InlineData(1, "Income")]
        [InlineData(2, "Transfer")]
        public void GetTypeString_PaymentType_EnumString(int typeInt, string expectedResult)
        {
            PaymentTypeHelper.GetTypeString(typeInt).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        public void GetTypeString_InvalidType_Exception(int typeInt)
        {
            var exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => PaymentTypeHelper.GetTypeString(typeInt));
            exception.Message.StartsWith("Passed Number didn't match to a payment type.").ShouldBeTrue();
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend)]
        [InlineData(PaymentRecurrence.Weekly)]
        [InlineData(PaymentRecurrence.Yearly)]
        public void GetPaymentFromRecurring_RecurringPayment_CorrectMappedFinancialTrans(
            PaymentRecurrence recurrence)
        {
            var account = new Account {Id = 2};

            var recurringPayment = new RecurringPayment
            {
                Id = 4,
                Recurrence = (int) recurrence,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            var result = RecurringPaymentHelper.GetPaymentFromRecurring(recurringPayment);

            result.ChargedAccount.ShouldBe(account);
            result.ChargedAccountId.ShouldBe(account.Id);
            result.Amount.ShouldBe(105);
            result.Date.ShouldBe(DateTime.Today);
        }

        [TestMethod]
        public void GetPaymentFromRecurring_MonthlyPayment_CorrectMappedFinancialTrans()
        {
            var account = new Account {Id = 2};
            var dayOfMonth = 26;

            var recurringPayment = new RecurringPayment
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Monthly,
                StartDate = new DateTime(2015, 08, dayOfMonth),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            var result = RecurringPaymentHelper.GetPaymentFromRecurring(recurringPayment);

            result.ChargedAccount.ShouldBe(account);
            result.ChargedAccountId.ShouldBe(account.Id);
            result.Amount.ShouldBe(105);
            result.Date.ShouldBe(new DateTime(DateTime.Today.Year, DateTime.Today.Month, dayOfMonth));
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily, 1)]
        [InlineData(PaymentRecurrence.Weekly, 7)]
        [InlineData(PaymentRecurrence.Monthly, 35)]
        [InlineData(PaymentRecurrence.Yearly, 380)]
        public void CheckIfRepeatable_DivRecurrences_ValidatedRecurrence(PaymentRecurrence recurrence,
            int amountOfDaysBack)
        {
            var account = new Account {Id = 2};

            var recurringPayment = new RecurringPayment
            {
                Id = 4,
                Recurrence = (int) recurrence,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new Payment {Date = DateTime.Today.AddDays(-amountOfDaysBack), IsCleared = true})
                .ShouldBeTrue();
        }

        [TestMethod]
        public void CheckIfRepeatable_UnclearedPayment_ReturnFalse()
        {
            var account = new Account {Id = 2};

            var recurringPayment = new RecurringPayment
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Weekly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new Payment {Date = DateTime.Today.AddDays(11)}).ShouldBeFalse();
        }
    }
}