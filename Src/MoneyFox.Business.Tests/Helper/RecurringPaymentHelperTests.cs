using System;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Helper
{
    public class RecurringPaymentHelperTests
    {
        [Theory]
        [InlineData(PaymentRecurrence.Daily, PaymentType.Income)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Weekly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Biweekly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Monthly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Bimonthly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Yearly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Daily, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Weekly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Biweekly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Monthly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Bimonthly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Yearly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Daily, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Weekly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Biweekly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Monthly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Bimonthly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Yearly, PaymentType.Transfer)]
        public void GetRecurringFromPayment_Endless(PaymentRecurrence recurrence, PaymentType type)
        {
            var startDate = new DateTime(2015, 03, 12);
            var enddate = Convert.ToDateTime("7.8.2016");

            var payment = new PaymentViewModel
            {
                ChargedAccount = new AccountViewModel {Id = 3},
                TargetAccount = new AccountViewModel {Id = 8},
                Category = new CategoryViewModel {Id = 16},
                Date = startDate,
                Amount = 2135,
                IsCleared = false,
                Type = type,
                IsRecurring = true
            };

            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, true,
                recurrence, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(true);
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
        public void GetTypeString_EnumString(int enumInt, string enumString)
        {
            PaymentTypeHelper.GetTypeString(enumInt).ShouldBe(enumString);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        public void GetTypeString_InvalidType_Exception(int enumInt)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>PaymentTypeHelper.GetTypeString(enumInt));
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend)]
        [InlineData(PaymentRecurrence.Weekly)]
        [InlineData(PaymentRecurrence.Biweekly)]
        [InlineData(PaymentRecurrence.Monthly)]
        [InlineData(PaymentRecurrence.Bimonthly)]
        [InlineData(PaymentRecurrence.Yearly)]
        public void GetPaymentFromRecurring_CorrectMappedPayment(PaymentRecurrence recurrence)
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = recurrence,
                StartDate = new DateTime(2015, 08, DateTime.Today.Day),
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

        public void GetPaymentFromRecurring_MonthlyPayment_CorrectMappedPayment()
        {
            var account = new AccountViewModel { Id = 2 };
            var dayOfMonth = 26;

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = PaymentRecurrence.Monthly,
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
        [InlineData(PaymentRecurrence.Weekly, 8)]
        [InlineData(PaymentRecurrence.Biweekly, 14)]
        [InlineData(PaymentRecurrence.Monthly, 31)]
        [InlineData(PaymentRecurrence.Bimonthly, 62)]
        [InlineData(PaymentRecurrence.Yearly, 360)]
        public void CheckIfRepeatable_ValidatedRecurrence(PaymentRecurrence recurrence, int amountOfDaysPassed)
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = recurrence,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(-amountOfDaysPassed), IsCleared = true})
                .ShouldBeTrue();
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
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = PaymentRecurrence.Weekly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(amountOfDaysUntilRepeat)}).ShouldBeFalse();
        }
    }
}