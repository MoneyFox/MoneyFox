namespace MoneyFox.Core.Tests.Features.LedgerCreation;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Features.LedgerCreation;

public sealed class CreateLedgerHandlerTests : InMemoryTestBase
{
    private readonly CreateLedger.Handler handler;

    public CreateLedgerHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task CreatesLedgerCorrectly()
    {
        // Arrange
        var testLedger = new TestData.SavingsLedger();

        // Act
        var command = new CreateLedger.Command(testLedger.Name, testLedger.OpeningBalance, testLedger.Note, testLedger.IsExcludeFromEndOfMonthSummary);
        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbLedger = await Context.Ledgers.SingleAsync();
        dbLedger.Id.Value.Should().BeGreaterThan(0);
        dbLedger.Name.Should().Be(testLedger.Name);
        dbLedger.OpeningBalance.Should().Be(testLedger.OpeningBalance);
        dbLedger.Note.Should().Be(testLedger.Note);
        dbLedger.IsExcludeFromEndOfMonthSummary.Should().Be(testLedger.IsExcludeFromEndOfMonthSummary);
    }
}
