namespace MoneyFox.Tests.Core.Commands.Categories.CreateCategory
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.Commands.Categories.CreateCategory;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task CreateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            // Act
            await new CreateCategoryCommand.Handler(contextAdapterMock.Object).Handle(request: new CreateCategoryCommand("Test"), cancellationToken: default);

            // Assert
            Assert.Single(context.Categories);
        }

        [Fact]
        public async Task CreateCategoryCommand_ShouldSaveRequireNoteCorrectly()
        {
            // Arrange
            // Act
            await new CreateCategoryCommand.Handler(contextAdapterMock.Object).Handle(
                request: new CreateCategoryCommand(name: "Test", requireNote: true),
                cancellationToken: default);

            var loadedCategory = context.Categories.First();

            // Assert
            loadedCategory.RequireNote.Should().BeTrue();
        }
    }

}
