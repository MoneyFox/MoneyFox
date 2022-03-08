namespace MoneyFox.Core.Tests.Queries.Categories.GetCategoryById
{
    using Common.Interfaces;
    using Core.Aggregates.Payments;
    using Core.Queries.Categories.GetCategoryById;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task GetCategory_CategoryNotFound()
        {
            // Arrange

            // Act
            Category result =
                await new GetCategoryByIdQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetCategoryByIdQuery(999),
                    default);

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
            Category result =
                await new GetCategoryByIdQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetCategoryByIdQuery(testCat1.Id),
                    default);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(testCat1.Name);
        }
    }
}