namespace MoneyFox.Core.Tests.Queries;

using Core.Queries;
using Domain.Tests.TestFramework;
using FluentAssertions;

public sealed class GetLedgerListTests : InMemoryTestBase
{
    private readonly GetLedgerList.Handler handler;

    public GetLedgerListTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        var ledger = new TestData.SavingsLedger();
        var dbLedger = Context.RegisterLedger(ledger);

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        var ledgerData = result.Single();
        ledgerData.LedgerId.Should().Be(dbLedger.Id);
        ledgerData.Name.Should().Be(ledger.Name);
        ledgerData.CurrentBalance.Should().Be(ledger.OpeningBalance);
        ledgerData.CurrentBalance.Currency.Should().Be(ledger.OpeningBalance.Currency);
        ledgerData.IsExcludeFromEndOfMonthSummary.Should().Be(ledger.IsExcludeFromEndOfMonthSummary);
    }
}
