using System;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Exceptions;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Entities
{
    public class RecurringPaymentTests
    {
        [Fact]
        public void Ctor_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, "note", null));
        }

        [Fact]
        public void Ctor_EndDateNull_IsEndlessTrue()
        {
            // Arrange

            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, "note", new Account("Foo"));

            // Assert
            recurringPayment.IsEndless.ShouldBeTrue();
        }

        [Fact]
        public void Ctor_EndDateSet_IsEndlessFalse()
        {
            // Arrange

            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, "note", new Account("Foo"), DateTime.MaxValue);

            // Assert
            recurringPayment.IsEndless.ShouldBeFalse();
        }

        [Fact]
        public void Ctor_EndDatePast_Exception()
        {
            // Arrange

            // Act / Assert
            Assert.Throws<MoneyFoxInvalidEndDateException>(() 
                => new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, "note", new Account("Foo"), DateTime.Today.AddDays(-1)));
        }
    }
}
