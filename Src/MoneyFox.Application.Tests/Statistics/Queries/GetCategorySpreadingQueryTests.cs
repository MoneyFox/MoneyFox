using FluentAssertions;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries;

[ExcludeFromCodeCoverage]
[Collection("CultureCollection")]
public class GetCategorySpreadingQueryTests : IDisposable
{
    private readonly EfCoreContext context;
    private readonly Mock<IContextAdapter> contextAdapterMock;

    public GetCategorySpreadingQueryTests()
    {
        context = InMemoryEfCoreContextFactory.Create();

        contextAdapterMock = new Mock<IContextAdapter>();
        contextAdapterMock.SetupGet(x => x.Context).Returns(context);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

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
            new(DateTime.Today, 60, PaymentType.Income, account, category: testCat1),
            new(DateTime.Today, 90, PaymentType.Expense, account, category: testCat1),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: testCat3),
            new(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object)
                .Handle(new GetCategorySpreadingQuery(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)), default))
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
            new(DateTime.Today, 60, PaymentType.Income, account, category: testCat1),
            new(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: testCat3)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object)
                .Handle(new GetCategorySpreadingQuery(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)), default))
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
            new(DateTime.Today, 90, PaymentType.Expense, account, category: testCat1),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: testCat2)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object)
                .Handle(new GetCategorySpreadingQuery(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)), default))
            .ToList();

        // Assert
        result[0].Label.Should().Be(testCat1.Name);
        result[1].Label.Should().Be(testCat2.Name);
    }

    [Fact]
    public async Task GetValues_CorrectColor()
    {
        // Arrange
        var account = new Account("test");
        var paymentList = new List<Payment>
        {
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("a")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("b")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("c")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("d")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("e")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("f")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("g"))
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object)
                .Handle(new GetCategorySpreadingQuery(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)), default))
            .ToList();

        // Assert
        result[0].Color.Should().Be("#266489");
        result[1].Color.Should().Be("#68B9C0");
        result[2].Color.Should().Be("#90D585");
        result[3].Color.Should().Be("#F3C151");
        result[4].Color.Should().Be("#F37F64");
        result[5].Color.Should().Be("#424856");
        result[6].Color.Should().Be("#8F97A4");
    }

    [Theory]
    [InlineData("en-US", '$')]
    [InlineData("de-CH", 'C')]
    public async Task GetValues_CorrectCurrency(string culture, char expectedCurrencySymbol)
    {
        // Arrange
        var cultureInfo = new CultureInfo(culture);
        CultureHelper.CurrentCulture = cultureInfo;

        var account = new Account("test");
        var paymentList = new List<Payment>
        {
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("a")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("b")),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("c"))
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object)
                .Handle(new GetCategorySpreadingQuery(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)), default))
            .ToList();

        // Assert
        result[0].ValueLabel[0].Should().Be(expectedCurrencySymbol);
        result[1].ValueLabel[0].Should().Be(expectedCurrencySymbol);
        result[2].ValueLabel[0].Should().Be(expectedCurrencySymbol);
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
            new(DateTime.Today, 90, PaymentType.Income, account, category: testCat1),
            new(DateTime.Today, 60, PaymentType.Expense, account, category: testCat1),
            new(DateTime.Today, 10, PaymentType.Expense, account, category: testCat2),
            new(DateTime.Today, 90, PaymentType.Income, account, category: testCat2)
        };

        context.Payments.AddRange(paymentList);
        context.SaveChanges();

        // Act
        var result = (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object)
                .Handle(
                    new GetCategorySpreadingQuery(
                        DateTime.Today.AddDays(-3),
                        DateTime.Today.AddDays(3),
                        PaymentType.Income),
                    default))
            .ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Value.Should().Be(80);
        result[1].Value.Should().Be(30);
    }
}