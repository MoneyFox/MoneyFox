namespace MoneyFox.Core.Tests.Queries.Accounts.GetAccountNameById
{
    using Core._Pending_.Common.Interfaces;
    using Core.Aggregates;
    using Core.Queries.Accounts.GetAccountNameById;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account("test2", 80);
            await context.AddAsync(account1);
            await context.SaveChangesAsync();

            // Act
            string result =
                await new GetAccountNameByIdQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetAccountNameByIdQuery(account1.Id),
                    default);

            // Assert
            result.Should().Be(account1.Name);
        }

        [Fact]
        public async Task EmptyStringWhenNoAccountFound()
        {
            // Arrange
            // Act
            string result =
                await new GetAccountNameByIdQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetAccountNameByIdQuery(33),
                    default);

            // Assert
            result.Should().Be(string.Empty);
        }
    }
}