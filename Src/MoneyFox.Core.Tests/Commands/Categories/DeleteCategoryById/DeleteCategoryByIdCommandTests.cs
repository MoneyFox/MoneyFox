namespace MoneyFox.Core.Tests.Commands.Categories.DeleteCategoryById
{

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeleteCategoryByIdCommandTests
    {
        private readonly AppDbContext context;
        private readonly DeleteCategoryByIdCommand.Handler handler;

        public DeleteCategoryByIdCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new DeleteCategoryByIdCommand.Handler(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("test");
            await context.AddAsync(category1);
            await context.SaveChangesAsync();

            // Act
            await handler.Handle(request: new DeleteCategoryByIdCommand(category1.Id), cancellationToken: default);

            // Assert
            (await context.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id)).Should().BeNull();
        }
    }

}
