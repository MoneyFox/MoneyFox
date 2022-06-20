namespace MoneyFox.Tests.Core.ApplicationCore.Queries
{

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Queries.BudgetEntryLoading;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    public class LoadBudgetEntryShould
    {
        private readonly AppDbContext appDbContext;
            private readonly LoadBudgetEntry.Handler handler;

        protected LoadBudgetEntryShould()
        {
            appDbContext = InMemoryAppDbContextFactory.Create();
            handler = new LoadBudgetEntry.Handler(appDbContext);
        }

        public class GivenNoBudgets : LoadBudgetEntryShould
        {
            [Fact]
            public void ThrowException()
            {
                // Act
                var query = new LoadBudgetEntry.Query(10);
                Func<Task> act = () => handler.Handle(request: query, cancellationToken: CancellationToken.None);

                // Assert
                act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        public class GivenBudgetsExist : LoadBudgetEntryShould
        {
            [Fact]
            public async Task ReturnBudgetWithCorrectId()
            {
                // Arrange
                var testBudget = new TestData.DefaultBudget();
                var dbBudget = appDbContext.RegisterBudget(testBudget);

                // Act
                var query = new LoadBudgetEntry.Query(dbBudget.Id);
                var budgetEntryData = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

                // Assert
                budgetEntryData.Id.Should().Be(dbBudget.Id);
                budgetEntryData.Name.Should().Be(dbBudget.Name);
                budgetEntryData.SpendingLimit.Should().Be(dbBudget.SpendingLimit);

                //budgetEntryData.Categories.Should().BeEquivalentTo(testBudget.Categories.Select(id => new BudgetEntryData.BudgetCategory(id, "")));
            }
        }
    }

}
