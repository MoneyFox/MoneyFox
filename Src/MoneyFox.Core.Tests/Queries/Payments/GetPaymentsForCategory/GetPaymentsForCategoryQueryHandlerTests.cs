namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentsForCategory;

using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;

public class GetPaymentsForCategoryQueryHandlerTests : InMemoryTestBase
{
    private readonly GetPaymentsForCategorySummary.Handler handler;

    public GetPaymentsForCategoryQueryHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task CorrectPaymentsSelected()
    {
        // Arrange
        var account = new Account("asdf");
        var category = new Category("Test");
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
        var payment2 = new Payment(
            date: DateTime.Now,
            amount: 30,
            type: PaymentType.Expense,
            chargedAccount: account,
            category: category);

        var payment3 = new Payment(
            date: DateTime.Now,
            amount: 40,
            type: PaymentType.Expense,
            chargedAccount: account,
            category: category);

        Context.Add(payment1);
        Context.Add(payment2);
        Context.Add(payment3);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(categoryId: category.Id, dateRangeFrom: DateTime.Now.AddDays(-1), dateRangeTo: DateTime.Now.AddDays(1)),
            cancellationToken: default);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CorrectPaymentsSelectedWithNoCategory()
    {
        // Arrange
        var account = new Account("asdf");
        var category = new Category("Test");
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
        var payment2 = new Payment(
            date: DateTime.Now,
            amount: 30,
            type: PaymentType.Expense,
            chargedAccount: account,
            category: category);

        var payment3 = new Payment(
            date: DateTime.Now,
            amount: 40,
            type: PaymentType.Expense,
            chargedAccount: account,
            category: category);

        Context.Add(payment1);
        Context.Add(payment2);
        Context.Add(payment3);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(categoryId: 0, dateRangeFrom: DateTime.Now.AddDays(-1), dateRangeTo: DateTime.Now.AddDays(1)),
            cancellationToken: default);

        // Assert
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task IgnoresTransfers()
    {
        // Arrange
        var account = new Account("asdf");
        var category = new Category("Test");
        var payment1 = new Payment(
            date: DateTime.Now,
            amount: 30,
            type: PaymentType.Transfer,
            chargedAccount: account,
            category: category);

        var payment2 = new Payment(
            date: DateTime.Now,
            amount: 30,
            type: PaymentType.Expense,
            chargedAccount: account,
            category: category);

        var payment3 = new Payment(
            date: DateTime.Now,
            amount: 40,
            type: PaymentType.Expense,
            chargedAccount: account,
            category: category);

        Context.Add(payment1);
        Context.Add(payment2);
        Context.Add(payment3);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(categoryId: category.Id, dateRangeFrom: DateTime.Now.AddDays(-1), dateRangeTo: DateTime.Now.AddDays(1)),
            cancellationToken: default);

        // Assert
        result.Should().HaveCount(2);
    }
}
