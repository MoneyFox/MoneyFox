namespace MoneyFox.Core.Tests.Queries.Statistics.GetCashFlowQueryHandler;

using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;

public class GetCashFlowHandlerTests : InMemoryTestBase
{
    private readonly GetCashFlow.Handler getCashFlowQueryHandler;

    public GetCashFlowHandlerTests()
    {
        getCashFlowQueryHandler = new(Context);
    }

    [Fact]
    public async Task GetValues_CorrectSums()
    {
        // Arrange
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: new("Foo1")),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Income, chargedAccount: new("Foo2")),
                new(date: DateTime.Today, amount: 50, type: PaymentType.Expense, chargedAccount: new("Foo3")),
                new(date: DateTime.Today, amount: 40, type: PaymentType.Expense, chargedAccount: new("Foo3"))
            });

        await Context.SaveChangesAsync();

        // Act
        var result = await getCashFlowQueryHandler.Handle(
            request: new(StartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(-3), EndDate: DateOnly.FromDateTime(DateTime.Today).AddDays(3)),
            cancellationToken: default);

        // Assert
        result.Income.Should().Be(80);
        result.Expense.Should().Be(90);
        result.Gain.Should().Be(-10);
    }
}
