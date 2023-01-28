namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Payments.GetPaymentsForCategory;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

public class GetPaymentsForCategoryQueryHandlerTests
{
    private readonly AppDbContext context;
    private readonly GetPaymentsForCategorySummary.Handler handler;

    public GetPaymentsForCategoryQueryHandlerTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
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

        context.Add(payment1);
        context.Add(payment2);
        context.Add(payment3);
        await context.SaveChangesAsync();

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

        context.Add(payment1);
        context.Add(payment2);
        context.Add(payment3);
        await context.SaveChangesAsync();

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

        context.Add(payment1);
        context.Add(payment2);
        context.Add(payment3);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(categoryId: category.Id, dateRangeFrom: DateTime.Now.AddDays(-1), dateRangeTo: DateTime.Now.AddDays(1)),
            cancellationToken: default);

        // Assert
        result.Should().HaveCount(2);
    }
}
