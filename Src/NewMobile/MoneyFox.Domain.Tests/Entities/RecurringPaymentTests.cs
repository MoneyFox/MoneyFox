using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
using Should;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Domain.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class RecurringPaymentTests
    {
        [Fact]
        public void Ctor_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange
            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new RecurringPayment(DateTime.Now,
                                                                            123,
                                                                            PaymentType.Expense,
                                                                            PaymentRecurrence.Daily,
                                                                            null,
                                                                            "note"));
        }

        [Fact]
        public void Ctor_DefaultValuesSet()
        {
            // Arrange
            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now,
                                                        123,
                                                        PaymentType.Expense,
                                                        PaymentRecurrence.Daily,
                                                        new Account("Foo"),
                                                        "note");

            // Assert
            recurringPayment.LastRecurrenceCreated.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
            recurringPayment.ModificationDate.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
            recurringPayment.CreationTime.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
        }

        [Fact]
        public void Ctor_Params_ValuesAssigned()
        {
            // Arrange
            DateTime startDate = DateTime.Now;
            const int amount = 123;
            const PaymentType type = PaymentType.Expense;
            const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
            var account = new Account("foo");
            const string note = "asdf";

            // Act
            var recurringPayment = new RecurringPayment(startDate,
                                                        amount,
                                                        type,
                                                        recurrence,
                                                        account,
                                                        note);

            // Assert
            recurringPayment.StartDate.ShouldEqual(startDate);
            recurringPayment.IsEndless.ShouldBeTrue();
            recurringPayment.Amount.ShouldEqual(amount);
            recurringPayment.Type.ShouldEqual(type);
            recurringPayment.Recurrence.ShouldEqual(recurrence);
            recurringPayment.ChargedAccount.ShouldEqual(account);
            recurringPayment.Note.ShouldEqual(note);
        }

        [Fact]
        public void Ctor_EndDateNull_IsEndlessTrue()
        {
            // Arrange
            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now,
                                                        123,
                                                        PaymentType.Expense,
                                                        PaymentRecurrence.Daily,
                                                        new Account("Foo"),
                                                        "note");

            // Assert
            recurringPayment.IsEndless.ShouldBeTrue();
        }

        [Fact]
        public void Ctor_EndDateSet_IsEndlessFalse()
        {
            // Arrange
            // Act
            var recurringPayment = new RecurringPayment(DateTime.Now,
                                                        123,
                                                        PaymentType.Expense,
                                                        PaymentRecurrence.Daily,
                                                        new Account("Foo"),
                                                        "note",
                                                        DateTime.Today);

            // Assert
            recurringPayment.IsEndless.ShouldBeFalse();
            recurringPayment.EndDate.ShouldEqual(DateTime.Today);
        }

        [Fact]
        public void Ctor_EndDatePast_Exception()
        {
            // Arrange
            // Act / Assert
            Assert.Throws<InvalidEndDateException>(() => new RecurringPayment(DateTime.Now,
                                                                              123,
                                                                              PaymentType.Expense,
                                                                              PaymentRecurrence.Daily,
                                                                              new Account("Foo"),
                                                                              "note",
                                                                              DateTime.Today.AddDays(-1)));
        }

        [Fact]
        public void UpdateRecurringPayment_ValuesAssigned()
        {
            // Arrange
            DateTime startDate = DateTime.Now.AddDays(-1);
            DateTime endDate = DateTime.Now;
            const int amount = 123;
            const PaymentType type = PaymentType.Income;
            const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
            var account = new Account("foo");
            const string note = "asdf";

            var recurringPayment = new RecurringPayment(startDate,
                                                        65,
                                                        type,
                                                        PaymentRecurrence.Monthly,
                                                        new Account("1111"),
                                                        "foo");

            // Act
            recurringPayment.UpdateRecurringPayment(amount, recurrence, account, note, endDate);

            // Assert
            recurringPayment.StartDate.ShouldEqual(startDate);
            recurringPayment.EndDate.ShouldEqual(endDate);
            recurringPayment.Amount.ShouldEqual(amount);
            recurringPayment.Type.ShouldEqual(type);
            recurringPayment.Recurrence.ShouldEqual(recurrence);
            recurringPayment.ChargedAccount.ShouldEqual(account);
            recurringPayment.Note.ShouldEqual(note);
        }

        [Fact]
        public void UpdateRecurringPayment_CreationTimeSame()
        {
            // Arrange
            DateTime startDate = DateTime.Now.AddDays(-1);
            DateTime endDate = DateTime.Now;
            const int amount = 123;
            const PaymentType type = PaymentType.Income;
            const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
            var account = new Account("foo");
            const string note = "asdf";

            var recurringPayment = new RecurringPayment(startDate,
                                                        65,
                                                        type,
                                                        PaymentRecurrence.Monthly,
                                                        new Account("1111"),
                                                        "foo");

            DateTime creationTime = recurringPayment.CreationTime;

            // Act
            recurringPayment.UpdateRecurringPayment(amount, recurrence, account, note, endDate);

            // Assert
            recurringPayment.CreationTime.ShouldEqual(creationTime);
        }

        [Fact]
        public void UpdateRecurringPayment_EndDateNull_IsEndless()
        {
            // Arrange
            var recurringPayment = new RecurringPayment(DateTime.Now.AddDays(-1),
                                                        65,
                                                        PaymentType.Income,
                                                        PaymentRecurrence.Monthly,
                                                        new Account("1111"),
                                                        "foo");

            // Act
            recurringPayment.UpdateRecurringPayment(123, PaymentRecurrence.Daily, new Account("123"));

            // Assert
            recurringPayment.IsEndless.ShouldBeTrue();
        }

        [Fact]
        public void UpdateRecurringPayment_EndDateSet_IsEndlessFalse()
        {
            // Arrange
            var recurringPayment = new RecurringPayment(DateTime.Now.AddDays(-1),
                                                        65,
                                                        PaymentType.Income,
                                                        PaymentRecurrence.Monthly,
                                                        new Account("1111"),
                                                        "foo");

            // Act
            recurringPayment.UpdateRecurringPayment(123, PaymentRecurrence.Daily, new Account("123"), string.Empty, DateTime.Now);

            // Assert
            recurringPayment.IsEndless.ShouldBeFalse();
        }

        [Fact]
        public void SetLastRecurrenceCreatedDateUpdated()
        {
            // Arrange
            DateTime startDate = DateTime.Now;
            const int amount = 123;
            const PaymentType type = PaymentType.Expense;
            const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
            var account = new Account("foo");
            const string note = "asdf";

            var recurringPayment = new RecurringPayment(startDate,
                                                        amount,
                                                        type,
                                                        recurrence,
                                                        account,
                                                        note);
            // Act
            recurringPayment.SetLastRecurrenceCreatedDate();

            // Assert
            recurringPayment.LastRecurrenceCreated.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
            recurringPayment.ModificationDate.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
        }
    }
}
