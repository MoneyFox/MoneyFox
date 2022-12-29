namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Statistics;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries.Statistics.GetCategorySummary;
using FluentAssertions;
using Infrastructure.Persistence;
using Resources;
using TestFramework;

[ExcludeFromCodeCoverage]
public class GetCategorySummaryQueryTests
{
    private readonly AppDbContext context;
    private readonly GetCategorySummaryQueryHandler handler;

    public GetCategorySummaryQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetValues_CorrectSums()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        var testCat2 = new Category("Rent");
        var testCat3 = new Category("Food");
        var testCat4 = new Category("Income");
        var account = new Account("test");
        var paymentList = new List<Payment>
        {
            new(
                date: DateTime.Today,
                amount: 60,
                type: PaymentType.Income,
                chargedAccount: account,
                category: testCat1),
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat1),
            new(
                date: DateTime.Today,
                amount: 10,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat3),
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat2),
            new(
                date: DateTime.Today,
                amount: 100,
                type: PaymentType.Income,
                chargedAccount: account,
                category: testCat4)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = await handler.Handle(
            request: new() { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems.Count.Should().Be(4);
        result.CategoryOverviewItems[0].Value.Should().Be(-90);
        result.CategoryOverviewItems[1].Value.Should().Be(-30);
        result.CategoryOverviewItems[2].Value.Should().Be(-10);
        result.CategoryOverviewItems[3].Value.Should().Be(100);
    }

    [Fact]
    public async Task GetValues_CorrectLabels()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        var testCat2 = new Category("Rent");
        var testCat3 = new Category("Food");
        var testCat4 = new Category("Income");
        var account = new Account("test");
        var paymentList = new List<Payment>
        {
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat1),
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat2),
            new(
                date: DateTime.Today,
                amount: 10,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat3),
            new(
                date: DateTime.Today,
                amount: 100,
                type: PaymentType.Income,
                chargedAccount: account,
                category: testCat4)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = await handler.Handle(
            request: new() { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems[0].Label.Should().Be(testCat1.Name);
        result.CategoryOverviewItems[1].Label.Should().Be(testCat2.Name);
        result.CategoryOverviewItems[2].Label.Should().Be(testCat3.Name);
        result.CategoryOverviewItems[3].Label.Should().Be(testCat4.Name);
    }

    [Fact]
    public async Task GetValues_CorrectPercentage()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        var testCat2 = new Category("Ausgehen");
        var testCat3 = new Category("Income");
        var account = new Account("test");
        var paymentList = new List<Payment>
        {
            new(
                date: DateTime.Today,
                amount: 60,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat1),
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat2),
            new(
                date: DateTime.Today,
                amount: 100,
                type: PaymentType.Income,
                chargedAccount: account,
                category: testCat3)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = await handler.Handle(
            request: new() { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems[0].Percentage.Should().Be(60);
        result.CategoryOverviewItems[1].Percentage.Should().Be(40);
        result.CategoryOverviewItems[2].Percentage.Should().Be(100);
    }

    [Fact]
    public async Task GetValues_NoCategory_CorrectValue()
    {
        // Arrange
        var account = new Account("test");
        var paymentList = new List<Payment> { new(date: DateTime.Today, amount: 60, type: PaymentType.Expense, chargedAccount: account) };
        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = await handler.Handle(
            request: new() { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems[0].Value.Should().Be(-60);
    }

    [Fact]
    public async Task GetValues_NoCategory_CorrectLabel()
    {
        // Arrange
        var account = new Account("test");
        var paymentList = new List<Payment> { new(date: DateTime.Today, amount: 60, type: PaymentType.Expense, chargedAccount: account) };
        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = await handler.Handle(
            request: new() { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems[0].Label.Should().Be(Strings.NoCategoryLabel);
    }
}
