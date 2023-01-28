namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Statistics;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries.Statistics;
using FluentAssertions;
using Infrastructure.Persistence;

[Collection("CultureCollection")]
public class GetCashFlowQueryHandlerTests
{
    private readonly AppDbContext context;
    private readonly GetCashFlowQueryHandler getCashFlowQueryHandler;

    public GetCashFlowQueryHandlerTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        getCashFlowQueryHandler = new(context);
    }

    [Fact]
    public async Task GetValues_CorrectSums()
    {
        // Arrange
        context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: new("Foo1")),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Income, chargedAccount: new("Foo2")),
                new(date: DateTime.Today, amount: 50, type: PaymentType.Expense, chargedAccount: new("Foo3")),
                new(date: DateTime.Today, amount: 40, type: PaymentType.Expense, chargedAccount: new("Foo3"))
            });

        await context.SaveChangesAsync();

        // Act
        var result = await getCashFlowQueryHandler.Handle(
            request: new() { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
            cancellationToken: default);

        // Assert
        result.Income.Should().Be(80);
        result.Expense.Should().Be(90);
        result.Gain.Should().Be(-10);
    }
}
