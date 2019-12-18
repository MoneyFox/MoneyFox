using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Queries.GetIfCategoryWithNameExists
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task CategoryWithSameNameDontExist()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            await context.Categories.AddAsync(testCat1);
            await context.SaveChangesAsync();

            // Act
            bool result = await new GetIfCategoryWithNameExistsQuery.Handler(context).Handle(new GetIfCategoryWithNameExistsQuery("Foo"), default);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public async Task CategoryWithSameNameExist()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            await context.Categories.AddAsync(testCat1);
            await context.SaveChangesAsync();

            // Act
            bool result = await new GetIfCategoryWithNameExistsQuery.Handler(context).Handle(new GetIfCategoryWithNameExistsQuery(testCat1.Name), default);

            // Assert
            result.ShouldBeTrue();
        }
    }
}
