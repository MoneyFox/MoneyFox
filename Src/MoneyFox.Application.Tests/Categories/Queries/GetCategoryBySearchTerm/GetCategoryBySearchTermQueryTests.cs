using System.Threading.Tasks;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Queries.GetBySearchTerm
{
    public class GetCategoryBySearchTermQueryTests
    {
        private readonly EfCoreContext context;

        public GetCategoryBySearchTermQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }
        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("test");
            var category2 = new Category("test2");
            await context.AddAsync(category1);
            await context.AddAsync(category2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetCategoryBySearchTermQuery.Handler(context).Handle(new GetCategoryBySearchTermQuery(), default);

            // Assert
            result.Count.ShouldEqual(2);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("foo");
            var category2 = new Category("test2");
            await context.AddAsync(category1);
            await context.AddAsync(category2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetCategoryBySearchTermQuery.Handler(context).Handle(new GetCategoryBySearchTermQuery{SearchTerm = "Foo"}, default);

            // Assert
            Assert.Single(result);
        }
    }
}
