namespace MoneyFox.Domain.Tests.Aggregates.LedgerAggregate;

using Domain.Aggregates.LedgerAggregate;
using FluentAssertions;
using TestFramework;

public class LedgerTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void RejectCreationIfAccountNameIsInvalid(string name)
    {
        // Act
        var act = () => Ledger.Create(name, Money.Zero(Currencies.CHF));

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CorrectlyCreated()
    {
        // Arrange
        var testLedger = new TestData.SpendingLedger();

        // Act
        var ledger = Ledger.Create(testLedger.Name, testLedger.CurrentBalance, testLedger.Note, testLedger.IsExcluded);
    }
}
