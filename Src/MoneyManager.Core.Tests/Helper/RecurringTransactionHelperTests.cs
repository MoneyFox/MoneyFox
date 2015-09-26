using System;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Xunit;

namespace MoneyManager.Core.Tests.Helper
{
    public class RecurringTransactionHelperTests
    {
        [Theory]
        [InlineData(0, "7.8.2016", true, 1)]
        [InlineData(1, "7.8.2016", true, 2)]
        [InlineData(2, "7.8.2016", false, 3)]
        [InlineData(3, "7.8.2016", true, 1)]
        [InlineData(4, "7.8.2016", false, 3)]
        public void GetRecurringFromFinancialTransaction(int recurrence, string date, bool isEndless, int type)
        {
            var startDate = new DateTime(2015, 03, 12);
            var enddate = Convert.ToDateTime(date);

            var transaction = new FinancialTransaction
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

            var recurring = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction, isEndless,
                recurrence, enddate);

            recurring.ChargedAccount.Id.ShouldBe(3);
            recurring.TargetAccount.Id.ShouldBe(8);
            recurring.StartDate.ShouldBe(startDate);
            recurring.EndDate.ShouldBe(enddate);
            recurring.IsEndless.ShouldBe(isEndless);
            recurring.Amount.ShouldBe(transaction.Amount);
            recurring.Category.Id.ShouldBe(transaction.Category.Id);
            recurring.Type.ShouldBe(type);
            recurring.Recurrence.ShouldBe(recurrence);
            recurring.Note.ShouldBe(transaction.Note);
        }

        [Theory]
        [InlineData(0, "Spending")]
        [InlineData(1, "Income")]
        [InlineData(2, "Transfer")]
        public void GetTypeString_TransactionType_EnumString(int typeInt, string expectedResult)
        {
            TransactionTypeHelper.GetTypeString(typeInt).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        public void GetTypeString_InvalidType_Exception(int typeInt)
        {
            var exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => TransactionTypeHelper.GetTypeString(typeInt));
            exception.Message.StartsWith("Passed Number didn't match to a transaction type.").ShouldBeTrue();
        }

        [Theory]
        [InlineData(TransactionRecurrence.Daily)]
        [InlineData(TransactionRecurrence.DailyWithoutWeekend)]
        [InlineData(TransactionRecurrence.Weekly)]
        [InlineData(TransactionRecurrence.Yearly)]
        public void GetFinancialTransactionFromRecurring_RecurringTransaction_CorrectMappedFinancialTrans(
            TransactionRecurrence recurrence)
        {
            var account = new Account {Id = 2};

            var recTrans = new RecurringTransaction
            {
                Id = 4,
                Recurrence = (int) recurrence,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            var result = RecurringTransactionHelper.GetFinancialTransactionFromRecurring(recTrans);

            result.ChargedAccount.ShouldBe(account);
            result.ChargedAccountId.ShouldBe(account.Id);
            result.Amount.ShouldBe(105);
            result.Date.ShouldBe(DateTime.Today);
        }

        [Fact]
        public void GetFinancialTransactionFromRecurring_MonthlyTransaction_CorrectMappedFinancialTrans()
        {
            var account = new Account {Id = 2};
            var dayOfMonth = 26;

            var recTrans = new RecurringTransaction
            {
                Id = 4,
                Recurrence = (int) TransactionRecurrence.Monthly,
                StartDate = new DateTime(2015, 08, dayOfMonth),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            var result = RecurringTransactionHelper.GetFinancialTransactionFromRecurring(recTrans);

            result.ChargedAccount.ShouldBe(account);
            result.ChargedAccountId.ShouldBe(account.Id);
            result.Amount.ShouldBe(105);
            result.Date.ShouldBe(new DateTime(DateTime.Today.Year, DateTime.Today.Month, dayOfMonth));
        }

        [Theory]
        [InlineData(TransactionRecurrence.Daily, 1)]
        [InlineData(TransactionRecurrence.Weekly, 7)]
        [InlineData(TransactionRecurrence.Monthly, 35)]
        [InlineData(TransactionRecurrence.Yearly, 380)]
        public void CheckIfRepeatable_DivRecurrences_ValidatedRecurrence(TransactionRecurrence recurrence,
            int amountOfDaysBack)
        {
            var account = new Account {Id = 2};

            var recTrans = new RecurringTransaction
            {
                Id = 4,
                Recurrence = (int) recurrence,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringTransactionHelper.CheckIfRepeatable(recTrans,
                new FinancialTransaction {Date = DateTime.Today.AddDays(-amountOfDaysBack)});
        }
    }
}