namespace MoneyFox.Infrastructure.Tests.DataAccess;

using System.Collections.Immutable;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.Tests.TestFramework;
using FluentAssertions;
using Infrastructure.DataAccess;
using Persistence;
using static Core.Tests.TestFramework.BudgetAssertion;

public class BudgetRepositoryTests
{
    private readonly AppDbContext appDbContext;
    private readonly BudgetRepository budgetRepository;

    protected BudgetRepositoryTests()
    {
        appDbContext = InMemoryAppDbContextFactory.Create();
        budgetRepository = new(appDbContext);
    }

    public sealed class AddAsync : BudgetRepositoryTests
    {
        [Fact]
        public async Task AddsBudget()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();

            // Act
            await budgetRepository.AddAsync(testBudget.CreateDbBudget());

            // Assert
            appDbContext.Budgets.Should().ContainSingle();
            var loadedBudget = appDbContext.Budgets.Single();
            AssertBudget(actual: loadedBudget, expected: testBudget);
        }

        [Fact]
        public async Task AddsCategoryOnlyOnce_WhenAddIsCalledMultipleTimes()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            var testDbCategory = appDbContext.RegisterBudget(testBudget);

            // Act
            Func<Task> act = async () => await budgetRepository.AddAsync(testDbCategory);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("An item with the same key has already been added. Key: 1");
        }
    }

    public sealed class GetAsync : BudgetRepositoryTests
    {
        [Fact]
        public async Task CorrectBudget()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            var dbBudget = appDbContext.RegisterBudget(testBudget);

            // Act
            var loadedBudget = await budgetRepository.GetAsync(dbBudget.Id);

            // Assert
            AssertBudget(actual: loadedBudget, expected: testBudget);
        }
    }

    public sealed class UpdateAsync : BudgetRepositoryTests
    {
        [Fact]
        public async Task SaveUpdateCorrectly()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            var dbBudget = testBudget.CreateDbBudget();
            await budgetRepository.AddAsync(dbBudget);

            // Act
            dbBudget.Change(
                budgetName: "Updated Name",
                spendingLimit: new(500),
                includedCategories: ImmutableList.Create(33),
                timeRange: BudgetTimeRange.YearToDate);

            await budgetRepository.UpdateAsync(dbBudget);

            // Assert
            var loadedBudget = await budgetRepository.GetAsync(dbBudget.Id);
            loadedBudget.Name.Should().Be(dbBudget.Name);
            loadedBudget.SpendingLimit.Should().Be(dbBudget.SpendingLimit);
            loadedBudget.IncludedCategories.Should().BeEquivalentTo(dbBudget.IncludedCategories);
        }
    }

    public sealed class DeleteAsync : BudgetRepositoryTests
    {
        [Fact]
        public async Task SaveUpdateCorrectly()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            var dbBudget = testBudget.CreateDbBudget();
            await budgetRepository.AddAsync(dbBudget);

            // Act
            await budgetRepository.DeleteAsync(dbBudget.Id);

            // Assert
            appDbContext.Budgets.Should().BeEmpty();
        }
    }
}
