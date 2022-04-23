namespace MoneyFox.Tests.Core.Commands.Categories.UpdateCategory
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.Commands.Categories.UpdateCategory;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdateCategoryCommandTests()
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var category = new Category("test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData("foo");
            await new UpdateCategoryCommand.Handler(contextAdapterMock.Object).Handle(request: new UpdateCategoryCommand(category), cancellationToken: default);
            var loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.Name.Should().Be("foo");
        }

        [Fact]
        public async Task UpdateCategoryCommand_ShouldUpdateRequireNote()
        {
            // Arrange
            var category = new Category(name: "test", requireNote: false);
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData(name: "foo", requireNote: true);
            await new UpdateCategoryCommand.Handler(contextAdapterMock.Object).Handle(request: new UpdateCategoryCommand(category), cancellationToken: default);
            var loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.RequireNote.Should().BeTrue();
        }
    }

}
