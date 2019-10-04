using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Commands.CreateCategory
{
    [ExcludeFromCodeCoverage]
    public class CreateCategoryCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public CreateCategoryCommandTests()
        {
            context = TestEfCoreContextFactory.Create();
        }
        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task CreateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var category = new Category("test");

            // Act
            await new CreateCategoryCommand.Handler(context).Handle(new CreateCategoryCommand(category), default);

            // Assert
            Assert.Single(context.Categories);
        }
    }
}
