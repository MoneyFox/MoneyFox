using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Shared.Tests.Helper
{
    [TestClass]
    public class RecurringPaymentHelperTests
    {
        [TestMethod]
        public void GetRecurringFromPayment_DailyEndlessIncome()
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
                Type = PaymentType.Income,
                IsRecurring = true
            };

            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, true,
                (int) PaymentRecurrence.Daily, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(true);
            recurring.Amount.ShouldBe(payment.Amount);
            recurring.Category.Id.ShouldBe(payment.Category.Id);
            recurring.Type.ShouldBe(PaymentType.Income);
            recurring.Recurrence.ShouldBe((int) PaymentRecurrence.Daily);
            recurring.Note.ShouldBe(payment.Note);
        }

        [TestMethod]
        public void GetRecurringFromPayment_WeeklyEndlessExpense()
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
                Type = (int) PaymentType.Expense,
                IsRecurring = true
            };

            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, true,
                (int) PaymentRecurrence.Weekly, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(true);
            recurring.Amount.ShouldBe(payment.Amount);
            recurring.Category.Id.ShouldBe(payment.Category.Id);
            recurring.Type.ShouldBe(PaymentType.Expense);
            recurring.Recurrence.ShouldBe((int) PaymentRecurrence.Weekly);
            recurring.Note.ShouldBe(payment.Note);
        }

        [TestMethod]
        public void GetRecurringFromPayment_MonthlyEndlessTransfer()
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
                Type = PaymentType.Transfer,
                IsRecurring = true
            };

            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, true,
                (int) PaymentRecurrence.Monthly, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(true);
            recurring.Amount.ShouldBe(payment.Amount);
            recurring.Category.Id.ShouldBe(payment.Category.Id);
            recurring.Type.ShouldBe(PaymentType.Transfer);
            recurring.Recurrence.ShouldBe((int) PaymentRecurrence.Monthly);
            recurring.Note.ShouldBe(payment.Note);
        }

        [TestMethod]
        public void GetRecurringFromPayment_YearlyEndlessTransfer()
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
                Type = (int) PaymentType.Expense,
                IsRecurring = true
            };

            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, false,
                (int) PaymentRecurrence.Yearly, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(false);
            recurring.Amount.ShouldBe(payment.Amount);
            recurring.Category.Id.ShouldBe(payment.Category.Id);
            recurring.Type.ShouldBe(PaymentType.Expense);
            recurring.Recurrence.ShouldBe((int) PaymentRecurrence.Yearly);
            recurring.Note.ShouldBe(payment.Note);
        }

        [TestMethod]
        public void GetTypeString_Expense_EnumString()
        {
            PaymentTypeHelper.GetTypeString(0).ShouldBe("Expense");
        }

        [TestMethod]
        public void GetTypeString_Income_EnumString()
        {
            PaymentTypeHelper.GetTypeString(1).ShouldBe("Income");
        }

        [TestMethod]
        public void GetTypeString_Transfer_EnumString()
        {
            PaymentTypeHelper.GetTypeString(2).ShouldBe("Transfer");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTypeString_InvalidType_Exception()
        {
            PaymentTypeHelper.GetTypeString(3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTypeString_NegativeType_Exception()
        {
            PaymentTypeHelper.GetTypeString(-1);
        }

        [TestMethod]
        public void GetPaymentFromRecurring_MonthlyPayment_CorrectMappedFinancialTrans()
        {
            var account = new AccountViewModel {Id = 2};
            var dayOfMonth = 26;

            var recurringPayment = new RecurringPaymentViewModel
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

        [TestMethod]
        public void CheckIfRepeatable_Daily_ValidatedRecurrence()
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Daily,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(-1), IsCleared = true})
                .ShouldBeTrue();
        }

        [TestMethod]
        public void CheckIfRepeatable_Biweekly_ValidatedRecurrence()
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Biweekly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(-15), IsCleared = true})
                .ShouldBeTrue();
        }

        [TestMethod]
        public void CheckIfRepeatable_Weekly_ValidatedRecurrence()
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Weekly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(-7), IsCleared = true})
                .ShouldBeTrue();
        }

        [TestMethod]
        public void CheckIfRepeatable_Monthly_ValidatedRecurrence()
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Monthly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(-35), IsCleared = true})
                .ShouldBeTrue();
        }

        [TestMethod]
        public void CheckIfRepeatable_Yearly_ValidatedRecurrence()
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Yearly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(-380), IsCleared = true})
                .ShouldBeTrue();
        }

        [TestMethod]
        public void CheckIfRepeatable_UnclearedPayment_ReturnFalse()
        {
            var account = new AccountViewModel {Id = 2};

            var recurringPayment = new RecurringPaymentViewModel
            {
                Id = 4,
                Recurrence = (int) PaymentRecurrence.Weekly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(recurringPayment,
                new PaymentViewModel {Date = DateTime.Today.AddDays(11)}).ShouldBeFalse();
        }
    }
}