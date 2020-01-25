using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Queries.GetCategoryBySearchTerm
{
    [ExcludeFromCodeCoverage]
    public class GetCategoryBySearchTermQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCategoryBySearchTermQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

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
            List<Category> result =
                await new GetCategoryBySearchTermQuery.Handler(contextAdapterMock.Object).Handle(new GetCategoryBySearchTermQuery(),
                                                                                                 default);

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
                await new GetCategoryBySearchTermQuery.Handler(contextAdapterMock.Object).Handle(new GetCategoryBySearchTermQuery("guid"),
                                                                                                 default);

            // Assert
            Assert.Single(result);
        }
    }
}
