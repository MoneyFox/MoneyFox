using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Queries.GetCategoryBySearchTerm
{
    [ExcludeFromCodeCoverage]
    public class GetCategoryBySearchTermQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetCategoryBySearchTermQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
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
            List<Category> result = await new GetCategoryBySearchTermQuery.Handler(context).Handle(new GetCategoryBySearchTermQuery(), default);

            // Assert
            result.Count.ShouldEqual(2);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("this is a guid");
            var category2 = new Category("test2");
            await context.AddAsync(category1);
            await context.AddAsync(category2);
            await context.SaveChangesAsync();

            // Act
            List<Category> result =
                await new GetCategoryBySearchTermQuery.Handler(context).Handle(new GetCategoryBySearchTermQuery {SearchTerm = "guid"}, default);

            // Assert
            Assert.Single(result);
        }
    }
}
