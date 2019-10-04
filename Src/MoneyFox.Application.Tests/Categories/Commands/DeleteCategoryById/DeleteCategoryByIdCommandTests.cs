using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Commands.DeleteCategoryById
{
    [ExcludeFromCodeCoverage]
    public class DeleteCategoryByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public DeleteCategoryByIdCommandTests()
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
            await context.AddAsync(category1);
            await context.SaveChangesAsync();

            // Act
            await new DeleteCategoryByIdCommand.Handler(context).Handle(new DeleteCategoryByIdCommand(category1.Id), default);

            // Assert
            Assert.Single(context.Categories);
        }
    }
}
