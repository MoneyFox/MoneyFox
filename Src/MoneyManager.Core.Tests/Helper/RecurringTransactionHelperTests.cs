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
                Cleared = false,
                Type = type,
                IsRecurring = true
            };

            var recurring = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction, isEndless, recurrence, enddate);

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
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => TransactionTypeHelper.GetTypeString(typeInt));
            exception.Message.StartsWith("Passed Number didn't match to a transaction type.").ShouldBeTrue();
        }
    }
}
