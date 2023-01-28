namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Statistics;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries.Statistics;
using FluentAssertions;
using Infrastructure.Persistence;

[Collection("CultureCollection")]
public class GetCategorySpreadingQueryTests
{
    private readonly AppDbContext context;
    private readonly GetCategorySpreadingQueryHandler handler;

    public GetCategorySpreadingQueryTests()
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
                category: testCat2)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await handler.Handle(request: new(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)), cancellationToken: default))
            .ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Value.Should().Be(90);
        result[1].Value.Should().Be(30);
        result[2].Value.Should().Be(10);
    }

    [Fact]
    public async Task GetValues_IgnoreSingleIncomes()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        var testCat2 = new Category("Rent");
        var testCat3 = new Category("Food");
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
                category: testCat2),
            new(
                date: DateTime.Today,
                amount: 10,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat3)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await handler.Handle(request: new(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)), cancellationToken: default))
            .ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetValues_CorrectLabel()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        var testCat2 = new Category("Rent");
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
                amount: 10,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat2)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await handler.Handle(request: new(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)), cancellationToken: default))
            .ToList();

        // Assert
        result[0].CategoryName.Should().Be(testCat1.Name);
        result[1].CategoryName.Should().Be(testCat2.Name);
    }

    [Fact]
    public async Task CorrectSumsForIncome()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        var testCat2 = new Category("Rent");
        var account = new Account("test");
        var paymentList = new List<Payment>
        {
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Income,
                chargedAccount: account,
                category: testCat1),
            new(
                date: DateTime.Today,
                amount: 60,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat1),
            new(
                date: DateTime.Today,
                amount: 10,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: testCat2),
            new(
                date: DateTime.Today,
                amount: 90,
                type: PaymentType.Income,
                chargedAccount: account,
                category: testCat2)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await handler.Handle(
            request: new(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3), paymentType: PaymentType.Income),
            cancellationToken: default)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Value.Should().Be(80);
        result[1].Value.Should().Be(30);
    }
}
