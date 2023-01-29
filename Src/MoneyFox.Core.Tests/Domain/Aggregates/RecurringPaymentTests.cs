namespace MoneyFox.Core.Tests.Domain.Aggregates;

using Core.ApplicationCore.Domain.Aggregates;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Exceptions;
using FluentAssertions;

public class RecurringPaymentTests
{
    [Fact]
    public void Ctor_ChargedAccountNull_ArgumentNullException()
    {
        // Act / Assert
        // Arrange
        Assert.Throws<ArgumentNullException>(
            () => new RecurringPayment(
                startDate: DateTime.Now,
                amount: 123,
                type: PaymentType.Expense,
                recurrence: PaymentRecurrence.Daily,
                chargedAccount: null,
                isLastDayOfMonth: false,
                note: "note"));
    }

    [Fact]
    public void Ctor_DefaultValuesSet()
    {
        // Arrange
        // Act
        var recurringPayment = new RecurringPayment(
            startDate: DateTime.Now,
            amount: 123,
            type: PaymentType.Expense,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("Foo"),
            isLastDayOfMonth: false,
            note: "note");

        // Assert
        recurringPayment.LastRecurrenceCreated.Should().BeAfter(DateTime.Now.AddSeconds(-1));
    }

    [Fact]
    public void Ctor_Params_ValuesAssigned()
    {
        // Arrange
        var startDate = DateTime.Now;
        const int amount = 123;
        const PaymentType type = PaymentType.Expense;
        const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
        var account = new Account("foo");
        const string note = "asdf";

        // Act
        var recurringPayment = new RecurringPayment(
            startDate: startDate,
            amount: amount,
            type: type,
            recurrence: recurrence,
            chargedAccount: account,
            isLastDayOfMonth: true,
            note: note);

        // Assert
        recurringPayment.StartDate.Should().Be(startDate);
        recurringPayment.IsEndless.Should().BeTrue();
        recurringPayment.Amount.Should().Be(amount);
        recurringPayment.Type.Should().Be(type);
        recurringPayment.Recurrence.Should().Be(recurrence);
        recurringPayment.ChargedAccount.Should().Be(account);
        recurringPayment.IsLastDayOfMonth.Should().BeTrue();
        recurringPayment.Note.Should().Be(note);
    }

    [Fact]
    public void Ctor_EndDateNull_IsEndlessTrue()
    {
        // Arrange
        // Act
        var recurringPayment = new RecurringPayment(
            startDate: DateTime.Now,
            amount: 123,
            type: PaymentType.Expense,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("Foo"),
            isLastDayOfMonth: false,
            note: "note");

        // Assert
        recurringPayment.IsEndless.Should().BeTrue();
    }

    [Fact]
    public void Ctor_EndDateSet_IsEndlessFalse()
    {
        // Arrange
        // Act
        var recurringPayment = new RecurringPayment(
            startDate: DateTime.Now,
            amount: 123,
            type: PaymentType.Expense,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("Foo"),
            isLastDayOfMonth: false,
            note: "note",
            endDate: DateTime.Today);

        // Assert
        recurringPayment.IsEndless.Should().BeFalse();
        recurringPayment.EndDate.Should().Be(DateTime.Today);
    }

    [Fact]
    public void ShouldThrowAnExceptionWhenDateInvalid()
    {
        // Act / Assert
        // Arrange
        Assert.Throws<InvalidEndDateException>(
            () => new RecurringPayment(
                startDate: DateTime.Now,
                amount: 123,
                type: PaymentType.Expense,
                recurrence: PaymentRecurrence.Daily,
                chargedAccount: new("Foo"),
                isLastDayOfMonth: false,
                note: "note",
                endDate: DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void ShouldNotThrowExceptionWhenIsEndlessWithNullDateOnCtor()
    {
        // Arrange
        // Act
        var payment = new RecurringPayment(
            startDate: DateTime.Now,
            amount: 123,
            type: PaymentType.Expense,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("Foo"),
            isLastDayOfMonth: false,
            note: "note");

        // Assert
        payment.Should().NotBeNull();
    }

    [Fact]
    public void ShouldNotThrowExceptionWhenIsEndlessWithNullOnUpdate()
    {
        // Arrange
        var payment = new RecurringPayment(
            startDate: DateTime.Now,
            amount: 123,
            type: PaymentType.Expense,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("Foo"),
            isLastDayOfMonth: false,
            note: "note");

        // Act
        payment.UpdateRecurringPayment(
            amount: 111,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: payment.ChargedAccount,
            isLastDayOfMonth: false,
            endDate: null);

        // Assert
        payment.Should().NotBeNull();
    }

    [Fact]
    public void ShouldNotThrowExceptionWhenIsEndlessWithMinDateOnUpdate()
    {
        // Arrange
        var payment = new RecurringPayment(
            startDate: DateTime.Now,
            amount: 123,
            type: PaymentType.Expense,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("Foo"),
            isLastDayOfMonth: false,
            note: "note");

        // Act
        payment.UpdateRecurringPayment(
            amount: 111,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: payment.ChargedAccount,
            isLastDayOfMonth: false,
            endDate: DateTime.MinValue);

        // Assert
        payment.Should().NotBeNull();
    }

    [Fact]
    public void UpdateRecurringPayment_ValuesAssigned()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        const int amount = 123;
        const PaymentType type = PaymentType.Income;
        const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
        var account = new Account("foo");
        const string note = "asdf";
        var recurringPayment = new RecurringPayment(
            startDate: startDate,
            amount: 65,
            type: type,
            recurrence: PaymentRecurrence.Monthly,
            chargedAccount: new("1111"),
            isLastDayOfMonth: false,
            note: "foo");

        // Act
        recurringPayment.UpdateRecurringPayment(
            amount: amount,
            recurrence: recurrence,
            chargedAccount: account,
            note: note,
            isLastDayOfMonth: true,
            endDate: endDate);

        // Assert
        recurringPayment.StartDate.Should().Be(startDate);
        recurringPayment.EndDate.Should().Be(endDate);
        recurringPayment.Amount.Should().Be(amount);
        recurringPayment.Type.Should().Be(type);
        recurringPayment.Recurrence.Should().Be(recurrence);
        recurringPayment.ChargedAccount.Should().Be(account);
        recurringPayment.IsLastDayOfMonth.Should().BeTrue();
        recurringPayment.Note.Should().Be(note);
    }

    [Fact]
    public void UpdateRecurringPayment_EndDateNull_IsEndless()
    {
        // Arrange
        var recurringPayment = new RecurringPayment(
            startDate: DateTime.Now.AddDays(-1),
            amount: 65,
            type: PaymentType.Income,
            recurrence: PaymentRecurrence.Monthly,
            chargedAccount: new("1111"),
            isLastDayOfMonth: false,
            note: "foo");

        // Act
        recurringPayment.UpdateRecurringPayment(amount: 123, recurrence: PaymentRecurrence.Daily, chargedAccount: new("123"), isLastDayOfMonth: false);

        // Assert
        recurringPayment.IsEndless.Should().BeTrue();
    }

    [Fact]
    public void UpdateRecurringPayment_EndDateSet_IsEndlessFalse()
    {
        // Arrange
        var recurringPayment = new RecurringPayment(
            startDate: DateTime.Now.AddDays(-1),
            amount: 65,
            type: PaymentType.Income,
            recurrence: PaymentRecurrence.Monthly,
            chargedAccount: new("1111"),
            isLastDayOfMonth: false,
            note: "foo");

        // Act
        recurringPayment.UpdateRecurringPayment(
            amount: 123,
            recurrence: PaymentRecurrence.Daily,
            chargedAccount: new("123"),
            isLastDayOfMonth: false,
            note: string.Empty,
            endDate: DateTime.Now);

        // Assert
        recurringPayment.IsEndless.Should().BeFalse();
    }

    [Fact]
    public void SetLastRecurrenceCreatedDateUpdated()
    {
        // Arrange
        var startDate = DateTime.Now;
        const int amount = 123;
        const PaymentType type = PaymentType.Expense;
        const PaymentRecurrence recurrence = PaymentRecurrence.Daily;
        var account = new Account("foo");
        const string note = "asdf";
        var recurringPayment = new RecurringPayment(
            startDate: startDate,
            amount: amount,
            type: type,
            recurrence: recurrence,
            chargedAccount: account,
            isLastDayOfMonth: false,
            note: note);

        // Act
        recurringPayment.SetLastRecurrenceCreatedDate();

        // Assert
        recurringPayment.LastRecurrenceCreated.Should().BeAfter(DateTime.Now.AddSeconds(-1));
    }
}
