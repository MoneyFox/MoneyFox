using FluentAssertions;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Categories.UpdateCategory;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Categories.UpdateCategory
{
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var category = new Category("test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData("foo");
            await new UpdateCategoryCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new UpdateCategoryCommand(category), default);

            Category loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.Name.Should().Be("foo");
        }

        [Fact]
        public async Task UpdateCategoryCommand_ShouldUpdateRequireNote()
        {
            // Arrange
            var category = new Category("test", requireNote: false);
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData("foo", requireNote: true);
            await new UpdateCategoryCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new UpdateCategoryCommand(category), default);

            Category loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.RequireNote.Should().BeTrue();
        }
    }
}