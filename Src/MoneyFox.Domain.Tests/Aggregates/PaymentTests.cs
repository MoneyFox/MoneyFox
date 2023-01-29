namespace MoneyFox.Domain.Tests.Aggregates;

using Domain.Aggregates.AccountAggregate;
using Exceptions;
using FluentAssertions;

public class PaymentTests
{
    [Fact]
    public void Ctor_ChargedAccountNull_ArgumentNullException()
    {
        // Act / Assert
        // Arrange
        Assert.Throws<AccountNullException>(
            () => new Payment(
                date: DateTime.Now,
                amount: 123,
                type: PaymentType.Expense,
                chargedAccount: null,
                note: "note"));
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    public void Ctor_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
    {
        // Arrange
        var payment = new Payment(date: DateTime.Now.AddDays(daysToAdd), amount: 123, type: PaymentType.Expense, chargedAccount: new("foo"));

        // Act

        // Assert
        payment.IsCleared.Should().Be(expectedIsCleared);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    public void ClearPayment_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
    {
        // Arrange

        // Act
        var payment = new Payment(date: DateTime.Now.AddDays(daysToAdd), amount: 123, type: PaymentType.Expense, chargedAccount: new("foo"));
        payment.ClearPayment();

        // Assert
        payment.IsCleared.Should().Be(expectedIsCleared);
    }

    [Fact]
    public void UpdatePayment_ChargedAccountNull_ArgumentNullException()
    {
        // Arrange
        var testPayment = new Payment(date: DateTime.Now, amount: 123, type: PaymentType.Expense, chargedAccount: new("foo"));

        // Act / Assert
        Assert.Throws<AccountNullException>(
            () => testPayment.UpdatePayment(date: DateTime.Today, amount: 123, type: PaymentType.Expense, chargedAccount: null));
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    public void UpdatePayment_AddedDays_ClearedCorrect(int daysToAdd, bool expectedIsCleared)
    {
        // Arrange
        var testPayment = new Payment(date: DateTime.Now, amount: 123, type: PaymentType.Expense, chargedAccount: new("foo"));

        // Act
        testPayment.UpdatePayment(date: DateTime.Now.AddDays(daysToAdd), amount: 123, type: PaymentType.Expense, chargedAccount: new("foo"));

        // Assert
        testPayment.IsCleared.Should().Be(expectedIsCleared);
    }
}
