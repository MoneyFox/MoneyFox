namespace MoneyFox.Tests.Infrastructure.DataAccess
{

    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using MoneyFox.Infrastructure.DataAccess;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;
    using static TestFramework.BudgetAssertion;

    public class BudgetRepositoryTests
    {
        private readonly BudgetRepository budgetRepository;
        private readonly AppDbContext appDbContext;

        protected BudgetRepositoryTests()
        {
            appDbContext = InMemoryAppDbContextFactory.Create();
            budgetRepository = new BudgetRepository(appDbContext);
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
                    spendingLimit: new SpendingLimit(500),
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

}
