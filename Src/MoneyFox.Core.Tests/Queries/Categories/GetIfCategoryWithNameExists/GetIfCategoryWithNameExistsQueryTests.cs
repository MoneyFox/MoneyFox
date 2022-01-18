using FluentAssertions;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Queries.Categories.GetIfCategoryWithNameExists;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Categories.GetIfCategoryWithNameExists
{
    [ExcludeFromCodeCoverage]
    public class GetIfCategoryWithNameExistsQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetIfCategoryWithNameExistsQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

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