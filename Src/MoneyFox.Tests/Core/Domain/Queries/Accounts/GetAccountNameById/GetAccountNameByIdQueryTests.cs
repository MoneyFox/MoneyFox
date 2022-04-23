namespace MoneyFox.Tests.Core.Domain.Queries.Accounts.GetAccountNameById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetAccountNameByIdQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountNameByIdQueryTests()
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
            await context.AddAsync(account1);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetAccountNameByIdQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetAccountNameByIdQuery(account1.Id),
                cancellationToken: default);

            // Assert
            result.Should().Be(account1.Name);
        }

        [Fact]
        public async Task EmptyStringWhenNoAccountFound()
        {
            // Arrange
            // Act
            var result = await new GetAccountNameByIdQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetAccountNameByIdQuery(33),
                cancellationToken: default);

            // Assert
            result.Should().Be(string.Empty);
        }
    }

}
