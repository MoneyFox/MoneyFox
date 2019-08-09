using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Queries.GetCategoryById
{
    [ExcludeFromCodeCoverage]
    public class GetCategoryByIdQueryHandlerTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetCategoryByIdQueryHandlerTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetCategory_CategoryNotFound()
        {
            // Arrange

            // Act
            var result = await new Handler(context).Handle(new GetCategoryByIdQuery
            {
                CategoryId = 999
            }, default);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GetCategory_CategoryFound()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            await context.Categories.AddAsync(testCat1);
            await context.SaveChangesAsync();

            // Act
            var result = await new Handler(context).Handle(new GetCategoryByIdQuery
            {
                CategoryId = testCat1.Id
            }, default);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldEqual(testCat1.Name);
        }
    }
}
