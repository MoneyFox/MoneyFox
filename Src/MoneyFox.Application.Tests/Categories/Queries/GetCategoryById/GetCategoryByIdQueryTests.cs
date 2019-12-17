using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Queries.GetCategoryById
{
    [ExcludeFromCodeCoverage]
    public class GetCategoryByIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCategoryByIdQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetCategory_CategoryNotFound()
        {
            // Arrange

            // Act
            Category result = await new GetCategoryByIdQuery.Handler(contextAdapterMock.Object).Handle(new GetCategoryByIdQuery(999), default);

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
            Category result = await new GetCategoryByIdQuery.Handler(contextAdapterMock.Object).Handle(new GetCategoryByIdQuery(testCat1.Id), default);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldEqual(testCat1.Name);
        }
    }
}
