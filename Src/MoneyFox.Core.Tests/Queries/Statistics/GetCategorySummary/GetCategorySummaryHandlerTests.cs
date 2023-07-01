namespace MoneyFox.Core.Tests.Queries.Statistics.GetCategorySummary;

using Core.Queries.Statistics.GetCategorySummary;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;

public class GetCategorySummaryHandlerTests : InMemoryTestBase
{
    private readonly GetCategorySummary.Handler handler;

    public GetCategorySummaryHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task ReturnsCorrectSums()
    {
        // Arrange
        var testCat1 = new Category("Going Out");
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

        Context.Payments.AddRange(paymentList);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(StartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(-3), EndDate: DateOnly.FromDateTime(DateTime.Today).AddDays(3)),
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems.Count.Should().Be(4);
        result.CategoryOverviewItems[0].Label.Should().Be(testCat2.Name);
        result.CategoryOverviewItems[0].Value.Should().Be(-90);
        result.CategoryOverviewItems[0].Average.Should().Be(-7.5m);
        result.CategoryOverviewItems[1].Label.Should().Be(testCat1.Name);
        result.CategoryOverviewItems[1].Value.Should().Be(-30);
        result.CategoryOverviewItems[1].Average.Should().Be(-2.5m);
        result.CategoryOverviewItems[2].Label.Should().Be(testCat3.Name);
        result.CategoryOverviewItems[2].Value.Should().Be(-10);
        result.CategoryOverviewItems[2].Average.Should().Be(-0.83m);
        result.CategoryOverviewItems[3].Label.Should().Be(testCat4.Name);
        result.CategoryOverviewItems[3].Value.Should().Be(100);
        result.CategoryOverviewItems[3].Average.Should().Be(8.33m);
    }

    [Fact]
    public async Task ReturnsCorrectPercentage()
    {
        // Arrange
        var testCat1 = new Category("Going Out");
        var testCat2 = new Category("Food");
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

        Context.Payments.AddRange(paymentList);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(StartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(-3), EndDate: DateOnly.FromDateTime(DateTime.Today).AddDays(3)),
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
        Context.Payments.AddRange(paymentList);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(StartDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-3)), EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(3))),
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
        Context.Payments.AddRange(paymentList);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(StartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(-3), EndDate: DateOnly.FromDateTime(DateTime.Today).AddDays(3)),
            cancellationToken: default);

        // Assert
        result.CategoryOverviewItems[0].Label.Should().Be("-");
    }
}
