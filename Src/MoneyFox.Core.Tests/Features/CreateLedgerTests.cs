namespace MoneyFox.Core.Tests.Features;

using Core.Features.LedgerCreation;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public sealed class CreateLedgerTests : InMemoryTestBase
{
    private readonly CreateLedger.Handler handler;

    public CreateLedgerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task LedgerCreatedCorrectly()
    {
        // Arrange
        var testLedger = new TestData.SavingsLedger();

        // Act
        var command = new CreateLedger.Command(testLedger.Name, testLedger.CurrentBalance, testLedger.Note, testLedger.IsExcludeFromEndOfMonthSummary);
        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbLedger = await Context.Ledgers.SingleAsync();
        dbLedger.Id.Value.Should().BeGreaterThan(0);
        dbLedger.Name.Should().Be(testLedger.Name);
        dbLedger.CurrentBalance.Should().Be(testLedger.CurrentBalance);
        dbLedger.Note.Should().Be(testLedger.Note);
        dbLedger.IsExcludeFromEndOfMonthSummary.Should().Be(testLedger.IsExcludeFromEndOfMonthSummary);
        dbLedger.Transactions.Should().BeEmpty().And.NotBeNull();
    }
}
