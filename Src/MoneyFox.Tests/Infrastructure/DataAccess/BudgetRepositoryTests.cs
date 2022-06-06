namespace MoneyFox.Tests.Infrastructure.DataAccess
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
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
            public async Task ReturnsBudgetForId()
            {
                // Arrange
                var testBudget = new TestData.DefaultBudget();
                var dbBudget = testBudget.CreateDbBudget();
                await budgetRepository.AddAsync(dbBudget);

                // Act
                var result = await budgetRepository.GetAsync(dbBudget.Id);

                // Assert
                AssertBudget(actual: result, expected: testBudget);
            }

            [Fact]
            public async Task ReturnsAllBudgets()
            {
                // Arrange
                var testBudget = new TestData.DefaultBudget();
                var dbBudget1 = testBudget.CreateDbBudget();
                var dbBudget2 = testBudget.CreateDbBudget();
                await budgetRepository.AddAsync(dbBudget1);
                await budgetRepository.AddAsync(dbBudget2);

                // Act
                var budgetList = await budgetRepository.GetAsync();

                // Assert
                budgetList.Should().HaveCount(2);
                foreach (var budget in budgetList)
                {
                    AssertBudget(actual: budget, expected: testBudget);
                }
            }
        }
    }

}
