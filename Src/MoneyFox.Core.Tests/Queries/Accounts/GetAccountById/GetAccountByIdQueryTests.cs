namespace MoneyFox.Core.Tests.Queries.Accounts.GetAccountById
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
    public class GetAccountByIdQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountByIdQueryTests()
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
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account(name: "test2", initalBalance: 80);
            var account2 = new Account(name: "test3", initalBalance: 80);
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetAccountByIdQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetAccountByIdQuery(account1.Id),
                cancellationToken: default);

            // Assert
            result.Name.Should().Be(account1.Name);
        }
    }

}
