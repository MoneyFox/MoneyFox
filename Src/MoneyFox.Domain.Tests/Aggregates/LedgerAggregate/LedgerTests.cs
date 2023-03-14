namespace MoneyFox.Domain.Tests.Aggregates.LedgerAggregate;

using System.Collections.Immutable;
using Domain.Aggregates.LedgerAggregate;
using FluentAssertions;
using FluentAssertions.Execution;

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
        var testLedger = new TestData.SavingsLedger();

        // Act
        var ledger = Ledger.Create(testLedger.Name, testLedger.CurrentBalance, testLedger.Note, testLedger.IsExcludeFromEndOfMonthSummary);

        // Assert
        using (new AssertionScope())
        {
            ledger.Id.Should().Be(new LedgerId());
            ledger.Name.Should().Be(testLedger.Name);
            ledger.CurrentBalance.Should().Be(testLedger.CurrentBalance);
            ledger.Note.Should().Be(testLedger.Note);
            ledger.IsExcludeFromEndOfMonthSummary.Should().Be(testLedger.IsExcludeFromEndOfMonthSummary);
        }
    }
}
