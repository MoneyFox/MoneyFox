using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Commands.DeleteCategoryById
{
    [ExcludeFromCodeCoverage]
    public class DeleteCategoryByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public DeleteCategoryByIdCommandTests()
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
            await context.AddAsync(category1);
            await context.SaveChangesAsync();

            // Act
            await new DeleteCategoryByIdCommand.Handler(context).Handle(new DeleteCategoryByIdCommand(category1.Id), default);

            // Assert
            (await context.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id)).ShouldBeNull();
        }
    }
}
