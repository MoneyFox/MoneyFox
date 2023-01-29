namespace MoneyFox.Core.Tests.Queries.Payments.GetMonthlyIncome;

using Core.Common.Helpers;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using NSubstitute;

public class GetMonthlyIncomeQueryTests : InMemoryTestBase
{
    private readonly GetMonthlyIncomeQuery.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public GetMonthlyIncomeQueryTests()
    {
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        handler = new(appDbContext: Context, systemDateHelper: systemDateHelper);
    }

    [Fact]
    public async Task ReturnCorrectAmount()
    {
        // Arrange
        systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
        var account = new Account(name: "test", initialBalance: 80);
        var payment1 = new Payment(date: new(year: 2020, month: 09, day: 10), amount: 50, type: PaymentType.Income, chargedAccount: account);
        var payment2 = new Payment(date: new(year: 2020, month: 09, day: 18), amount: 20, type: PaymentType.Income, chargedAccount: account);
        var payment3 = new Payment(date: new(year: 2020, month: 09, day: 4), amount: 30, type: PaymentType.Expense, chargedAccount: account);
        await Context.AddAsync(payment1);
        await Context.AddAsync(payment2);
        await Context.AddAsync(payment3);
        await Context.SaveChangesAsync();

        // Act
        var sum = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        sum.Should().Be(70);
    }
}
