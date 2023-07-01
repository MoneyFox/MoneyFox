namespace MoneyFox.Core.Tests.Queries.Statistics.GetCategoryProgression;

using Core.Common.Settings;
using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;
using NSubstitute;

public class GetCategoryProgressionHandlerTests : InMemoryTestBase
{
    private readonly GetCategoryProgressionHandler handler;

    public GetCategoryProgressionHandlerTests()
    {
        var settingsFacade = Substitute.For<ISettingsFacade>();
        settingsFacade.DefaultCurrency.Returns("CHF");
        handler = new(appDbContext: Context, settingsFacade: settingsFacade);
    }

    [Fact]
    public async Task CalculateCorrectSums()
    {
        // Arrange
        var account = new Account("Foo1");
        var category = new Category("abcd");
        Context.AddRange(
            new List<Payment>
            {
                new(
                    date: DateTime.Today,
                    amount: 60,
                    type: PaymentType.Income,
                    chargedAccount: account,
                    category: category),
                new(
                    date: DateTime.Today,
                    amount: 20,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: category),
                new(
                    date: DateTime.Today.AddMonths(-1),
                    amount: 50,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: category),
                new(
                    date: DateTime.Today.AddMonths(-2),
                    amount: 40,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: category)
            });

        Context.Add(account);
        Context.Add(category);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(categoryId: category.Id, startDate: DateTime.Today.AddYears(-1), endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[12].Value.Should().Be(40);
        result[11].Value.Should().Be(-50);
        result[10].Value.Should().Be(-40);
    }
}
