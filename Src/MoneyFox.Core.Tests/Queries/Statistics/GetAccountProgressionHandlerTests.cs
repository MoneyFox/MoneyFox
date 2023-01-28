namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Statistics;

using Core.ApplicationCore.Queries.Statistics;
using FluentAssertions;
using MoneyFox.Domain.Aggregates.AccountAggregate;

[Collection("CultureCollection")]
public class GetAccountProgressionHandlerTests : InMemoryTestBase
{
    private readonly GetAccountProgressionHandler getAccountProgressionHandler;

    public GetAccountProgressionHandlerTests()
    {
        getAccountProgressionHandler = new(Context);
    }

    [Fact]
    public async Task CalculateCorrectSums()
    {
        // Arrange
        var account = new Account("Foo1");
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: account),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-1), amount: 50, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-2), amount: 40, type: PaymentType.Expense, chargedAccount: account)
            });

        Context.Add(account);
        await Context.SaveChangesAsync();

        // Act
        var result = await getAccountProgressionHandler.Handle(
            request: new(accountId: account.Id, startDate: DateTime.Today.AddYears(-1), endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[0].Value.Should().Be(40);
        result[1].Value.Should().Be(-50);
        result[2].Value.Should().Be(-40);
    }

    [Fact]
    public async Task GetValues_CorrectSums()
    {
        // Arrange
        var account = new Account("Foo1");
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: account),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-1), amount: 50, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-2), amount: 40, type: PaymentType.Expense, chargedAccount: account)
            });

        Context.Add(account);
        await Context.SaveChangesAsync();

        // Act
        var result = await getAccountProgressionHandler.Handle(
            request: new(accountId: account.Id, startDate: DateTime.Today.AddYears(-1), endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[0].Color.Should().Be("#87cefa");
        result[1].Color.Should().Be("#cd3700");
        result[2].Color.Should().Be("#cd3700");
    }
}
