namespace MoneyFox.Tests.Core.ApplicationCore.Queries
{

    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    public sealed class LoadBudgetsShould
    {
        private readonly AppDbContext dbContext;
        private readonly IContextAdapter contextAdapter;
        private readonly LoadBudgets.Handler handler;

        public LoadBudgetsShould()
        {
            var dbContext = InMemoryAppDbContextFactory.Create();
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
    }

}
