using FluentAssertions;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Domain.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class PaymentTests
    {
        [Fact]
        public void Ctor_ChargedAccountNull_ArgumentNullException() =>
            // Arrange

            // Act / Assert
            Assert.Throws<AccountNullException>(() => new Payment(DateTime.Now, 123, PaymentType.Expense, null, note: "note"));

        [Theory]
        [InlineData(1, false)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        public void Ctor_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
        {
            // Arrange
            var payment = new Payment(DateTime.Now.AddDays(daysToAdd), 123, PaymentType.Expense, new Account("foo"));

            // Act

            // Assert
            payment.IsCleared.Should().Be(expectedIsCleared);
        }

        [Fact]
        public void Ctor_NoParams_DefaultValueSet()
        {
            // Arrange

            // Act
            var payment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));

            // Assert
            payment.ModificationDate.Should().BeAfter(DateTime.Now.AddSeconds(-1));
            payment.CreationTime.Should().BeAfter(DateTime.Now.AddSeconds(-1));
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        public void ClearPayment_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
        {
            // Arrange

            // Act
            var payment = new Payment(DateTime.Now.AddDays(daysToAdd), 123, PaymentType.Expense, new Account("foo"));
            payment.ClearPayment();

            // Assert
            payment.IsCleared.Should().Be(expectedIsCleared);
        }

        [Fact]
        public void AccountBalanceCorrectAfterClearing()
        {
            // Arrange
            Account account = new("test", 500);
            var payment = new Payment(DateTime.Now, 123, PaymentType.Expense, account);

            // Act
            payment.ClearPayment();

            // Assert
            payment.AccountBalance.Should().Be(account.CurrentBalance);
        }

        [Fact]
        public void UpdatePayment_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange
            var testPayment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));

            // Act / Assert
            Assert.Throws<AccountNullException>(() => testPayment.UpdatePayment(DateTime.Today, 123, PaymentType.Expense, null));
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        public void UpdatePayment_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
        {
            // Arrange
            var testPayment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));

            // Act
            testPayment.UpdatePayment(DateTime.Now.AddDays(daysToAdd), 123, PaymentType.Expense, new Account("foo"));

            // Assert
            testPayment.IsCleared.Should().Be(expectedIsCleared);
        }

        [Fact]
        public void AccountBalanceCorrectlyUpdated()
        {
            // Arrange
            Account account = new("test", 200);
            var testPayment = new Payment(DateTime.Now, 100, PaymentType.Expense, account);

            // Act
            testPayment.UpdatePayment(DateTime.Now, 150, PaymentType.Expense, account);

            // Assert
            testPayment.AccountBalance.Should().Be(50);
        }

        [Fact]
        public void AccountBalanceCorrectlyAssigned()
        {
            // Arrange
            const decimal newBalance = 666;
            Account account = new("test", 200);
            Payment testPayment = new(DateTime.Now, 100, PaymentType.Expense, account);

            // Act
            testPayment.UpdateAccountBalance(newBalance);

            // Assert
            testPayment.AccountBalance.Should().Be(newBalance);
        }

        [Fact]
        public void DontClearPaymentTwice()
        {
            // Arrange
            Account account1 = new("test1", 100);
            Payment testPayment = new(DateTime.Now, 40, PaymentType.Expense, account1);

            // Act
            testPayment.ClearPayment();
            testPayment.ClearPayment();

            // Assert
            testPayment.AccountBalance.Should().Be(60);
        }

        [Fact]
        public void AccountBalanceCorrectlyAssignedWithTransfer()
        {
            // Arrange
            Account account1 = new("test1", 100);
            Account account2 = new("test2", 200);
            Payment testPayment = new(DateTime.Now, 40, PaymentType.Transfer, account1, account2);

            // Act
            testPayment.ClearPayment();

            // Assert
            testPayment.AccountBalance.Should().Be(60);
        }

        [Fact]
        public void AccountBalanceOnlySetWhenPaymentCleared()
        {
            // Arrange
            Account account1 = new("test1", 100);
            Payment testPayment = new(DateTime.Now.AddDays(1), 40, PaymentType.Expense, account1, account1);

            // Act
            testPayment.ClearPayment();

            // Assert
            testPayment.AccountBalance.Should().Be(0);
        }
    }
}
