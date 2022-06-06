namespace MoneyFox.Tests.Core.ApplicationCore.Queries
{

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using TestFramework.Budget;
    using Xunit;

    public sealed class LoadBudgetsShould
    {
        private readonly AppDbContext dbContext;
        private readonly IContextAdapter contextAdapter;
        private readonly LoadBudgets.Handler handler;

        public LoadBudgetsShould()
        {
            dbContext = InMemoryAppDbContextFactory.Create();
            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(dbContext);
            handler = new LoadBudgets.Handler(contextAdapter);
        }

        [Fact]
        public async Task ReturnEmpty_WhenNoBudgetsCreated()
        {
            // Act
            var query = new LoadBudgets.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnBudgets()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            dbContext.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgets.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            var loadedBudget = result.Single();
            loadedBudget.Name.Should().Be(testBudget.Name);
            loadedBudget.SpendingLimit.Should().BeApproximately(testBudget.SpendingLimit, 0.01m);
        }
    }

}
