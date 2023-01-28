namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Payments.GetMonthlyExpense;

using Core.ApplicationCore.Queries;
using Core.Common.Helpers;
using FluentAssertions;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using NSubstitute;

public class GetMonthlyExpenseQueryTests : InMemoryTestBase
{
    private readonly GetMonthlyExpenseQuery.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public GetMonthlyExpenseQueryTests()
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
        var payment1 = new Payment(date: new(year: 2020, month: 09, day: 03), amount: 50, type: PaymentType.Expense, chargedAccount: account);
        var payment2 = new Payment(date: new(year: 2020, month: 09, day: 04), amount: 20, type: PaymentType.Expense, chargedAccount: account);
        var payment3 = new Payment(date: new(year: 2020, month: 09, day: 04), amount: 30, type: PaymentType.Income, chargedAccount: account);
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
