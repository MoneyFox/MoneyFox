using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class PaymentTests
    {
        [Fact]
        public void Ctor_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Payment(DateTime.Now, 123, PaymentType.Expense, null, note: "note"));
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        public void Ctor_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
        {            
            // Arrange

            // Act
            var payment = new Payment(DateTime.Now.AddDays(daysToAdd), 123, PaymentType.Expense, new Account("foo"));

            // Assert
            payment.IsCleared.ShouldEqual(expectedIsCleared);
        }

        [Fact]
        public void Ctor_NoParams_DefaultValueSet()
        {
            // Arrange

            // Act
            var payment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));

            // Assert
            payment.CreationTime.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
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
            payment.IsCleared.ShouldEqual(expectedIsCleared);
        }

        [Fact]
        public void UpdatePayment_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange
            var testPayment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => testPayment.UpdatePayment(DateTime.Today, 123, PaymentType.Expense, null));
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
            testPayment.IsCleared.ShouldEqual(expectedIsCleared);
        }

        [Fact]
        public void AddRecurringPayment_ValuesSetCorrectly()
        {
            // Arrange
            var testPayment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));

            // Act
            testPayment.AddRecurringPayment(PaymentRecurrence.Bimonthly);

            // Assert
            testPayment.IsRecurring.ShouldBeTrue();
            testPayment.RecurringPayment.ShouldNotBeNull();
        }

        [Fact]
        public void RemoveRecurringPayment_ValuesSetCorrectly()
        {
            // Arrange
            var testPayment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("foo"));
            testPayment.AddRecurringPayment(PaymentRecurrence.Bimonthly);

            // Act
            testPayment.RemoveRecurringPayment();

            // Assert
            testPayment.IsRecurring.ShouldBeFalse();
            testPayment.RecurringPayment.ShouldBeNull();
        }
    }
}
