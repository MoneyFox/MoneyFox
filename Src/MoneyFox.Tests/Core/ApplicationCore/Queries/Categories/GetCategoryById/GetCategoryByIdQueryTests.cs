namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Categories.GetCategoryById
{

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetCategoryByIdQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetCategoryByIdQuery.Handler handler;

        public GetCategoryByIdQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetCategoryByIdQuery.Handler(context);
        }

        [Fact]
        public async Task GetCategory_CategoryNotFound()
        {
            // Arrange

            // Act
            var result = await handler.Handle(request: new GetCategoryByIdQuery(999), cancellationToken: default);

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
            var result = await handler.Handle(request: new GetCategoryByIdQuery(testCat1.Id), cancellationToken: default);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(testCat1.Name);
        }
    }

}
