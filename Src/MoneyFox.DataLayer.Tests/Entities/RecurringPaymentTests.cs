using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Exceptions;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class RecurringPaymentTests
    {
        [Fact]
        public void Ctor_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, null, "note"));
        }

        [Fact]
        public void Ctor_NoParams_DefaultValuesSet()
        {
            // Arrange

            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, new Account("Foo"), "note");

            // Assert
            recurringPayment.CreationTime.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
        }

        [Fact]
        public void Ctor_EndDateNull_IsEndlessTrue()
        {
            // Arrange

            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, new Account("Foo"), "note" );

            // Assert
            recurringPayment.IsEndless.ShouldBeTrue();
        }

        [Fact]
        public void Ctor_EndDateSet_IsEndlessFalse()
        {
            // Arrange

            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, new Account("Foo"), "note", DateTime.Today);

            // Assert
            recurringPayment.IsEndless.ShouldBeFalse();
            recurringPayment.EndDate.ShouldEqual(DateTime.Today);
        }

        [Fact]
        public void Ctor_EndDatePast_Exception()
        {
            // Arrange

            // Act / Assert
            Assert.Throws<MoneyFoxInvalidEndDateException>(() 
                => new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, new Account("Foo"), "note", DateTime.Today.AddDays(-1)));
        }
    }
}
