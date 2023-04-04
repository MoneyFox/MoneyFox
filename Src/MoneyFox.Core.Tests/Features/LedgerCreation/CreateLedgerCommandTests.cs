namespace MoneyFox.Core.Tests.Features.LedgerCreation;

using Core.Features.LedgerCreation;
using Domain;
using FluentAssertions;

public sealed class CreateLedgerCommandTests : InMemoryTestBase
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowsException_WhenNameIsEmpty(string ledgerName)
    {
        // Act
        var act = () => new CreateLedger.Command(
            name: ledgerName,
            currentBalance: Money.Zero(Currencies.CHF),
            note: string.Empty,
            isExcludeFromEndOfMonthSummary: false);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AcceptsNullOrEmptyNotes(string? note)
    {
        // Arrange
        var name = "Expense";
        var currentBalance = new Money(amount: 22, currency: Currencies.CHF);

        // Act
        var command = new CreateLedger.Command(name: name, currentBalance: currentBalance, note: note, isExcludeFromEndOfMonthSummary: true);

        // Assert
        command.Name.Should().Be(name);
        command.CurrentBalance.Should().Be(currentBalance);
        command.Note.Should().Be(note);
        command.IsExcludeFromEndOfMonthSummary.Should().BeTrue();
    }
}
