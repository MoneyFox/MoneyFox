namespace MoneyFox.Tests.Core.Domain.Queries.Categories.GetCategoryById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetCategoryByIdQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCategoryByIdQueryTests()
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
        public async Task GetCategory_CategoryNotFound()
        {
            // Arrange

            // Act
            var result = await new GetCategoryByIdQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetCategoryByIdQuery(999),
                cancellationToken: default);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCategory_CategoryFound()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            await context.Categories.AddAsync(testCat1);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetCategoryByIdQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetCategoryByIdQuery(testCat1.Id),
                cancellationToken: default);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(testCat1.Name);
        }
    }

}
