namespace MoneyFox.Core.Tests.Queries.Categories.GetCategoryBySearchTerm
{
    using Common.Interfaces;
    using Core.Aggregates.Payments;
    using Core.Queries.Categories.GetCategoryBySearchTerm;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetCategoryBySearchTermQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCategoryBySearchTermQueryTests()
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
                await new GetCategoryBySearchTermQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetCategoryBySearchTermQuery(),
                    default);

            // Assert
            result.Should().HaveCount(2);
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
                await new GetCategoryBySearchTermQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetCategoryBySearchTermQuery("guid"),
                    default);

            // Assert
            Assert.Single(result);
        }
    }
}