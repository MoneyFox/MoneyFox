namespace MoneyFox.Core.Tests.Queries.Categories.GetIfCategoryWithNameExists
{
    using Core.Aggregates.Payments;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Core.Queries;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetIfCategoryWithNameExistsQueryTests : IDisposable
    {
        private readonly AppDbContext context;

        public GetIfCategoryWithNameExistsQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task CategoryWithSameNameDontExist()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            await context.Categories.AddAsync(testCat1);
            await context.SaveChangesAsync();

            // Act
            bool result =
                await new GetIfCategoryWithNameExistsQuery.Handler(context).Handle(
                    new GetIfCategoryWithNameExistsQuery("Foo"),
                    default);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CategoryWithSameNameExist()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            await context.Categories.AddAsync(testCat1);
            await context.SaveChangesAsync();

            // Act
            bool result =
                await new GetIfCategoryWithNameExistsQuery.Handler(context).Handle(
                    new GetIfCategoryWithNameExistsQuery(testCat1.Name),
                    default);

            // Assert
            result.Should().BeTrue();
        }
    }
}