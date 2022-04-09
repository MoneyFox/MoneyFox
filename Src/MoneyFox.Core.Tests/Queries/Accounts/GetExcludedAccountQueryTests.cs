namespace MoneyFox.Core.Tests.Queries.Accounts
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Queries;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetExcludedAccountQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetExcludedAccountQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initalBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initalBalance: 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            var resultList = await new GetExcludedAccountQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetExcludedAccountQuery(),
                cancellationToken: default);

            // Assert
            resultList.Should().ContainSingle();
            resultList[0].CurrentBalance.Should().Be(80);
        }

        [Fact]
        public async Task DontLoadDeactivatedAccounts()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initalBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initalBalance: 80);
            var accountDeactivated = new Account(name: "test", initalBalance: 80);
            accountDeactivated.Deactivate();
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.AddAsync(accountDeactivated);
            await context.SaveChangesAsync();

            // Act
            var resultList = await new GetExcludedAccountQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetExcludedAccountQuery(),
                cancellationToken: default);

            // Assert
            resultList.Should().ContainSingle();
            resultList[0].CurrentBalance.Should().Be(80);
        }
    }

}
