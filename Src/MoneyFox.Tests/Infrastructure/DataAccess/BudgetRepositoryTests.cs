namespace MoneyFox.Tests.Infrastructure.DataAccess
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Infrastructure.DataAccess;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using TestFramework.Budget;
    using Xunit;
    using static TestFramework.Budget.BudgetAssertion;

    public class BudgetRepositoryTests
    {
        private readonly BudgetRepository budgetRepository;
        private readonly AppDbContext appDbContext;

        public BudgetRepositoryTests()
        {
            appDbContext = InMemoryAppDbContextFactory.Create();
            budgetRepository = new BudgetRepository(appDbContext);
        }

        [Fact]
        public async Task AddsBudget()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();

            // Act
            await budgetRepository.AddAsync(testBudget.CreateDbBudget());

            // Assert
            appDbContext.Budgets.Should().ContainSingle();
            var loadedCategory = appDbContext.Budgets.Single();
            AssertBudget(actual: loadedCategory, expected: testBudget);
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

}
