namespace MoneyFox.Core.Tests.Commands.Categories.CreateCategory
{
    using Common.Interfaces;
    using Core.Aggregates.Payments;
    using Core.Commands.Categories.CreateCategory;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreateCategoryCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public CreateCategoryCommandTests()
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
        public async Task CreateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            // Act
            await new CreateCategoryCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new CreateCategoryCommand("Test"), default);

            // Assert
            Assert.Single(context.Categories);
        }

        [Fact]
        public async Task CreateCategoryCommand_ShouldSaveRequireNoteCorrectly()
        {
            // Arrange
            // Act
            await new CreateCategoryCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new CreateCategoryCommand("Test", requireNote: true), default);

            Category loadedCategory = context.Categories.First();

            // Assert
            loadedCategory.RequireNote.Should().BeTrue();
        }
    }
}