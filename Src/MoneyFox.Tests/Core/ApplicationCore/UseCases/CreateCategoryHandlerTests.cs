namespace MoneyFox.Tests.Core.ApplicationCore.UseCases
{

    using System.Threading;
    using System.Threading.Tasks;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.UseCases.CategoryCreation;
    using NSubstitute;
    using TestFramework.Category;
    using Xunit;
    using static TestFramework.Category.CategoryAssertion;

    public class CreateCategoryHandlerTests
    {
        private readonly ICategoryRepository repository;

        public CreateCategoryHandlerTests()
        {
            repository = Substitute.For<ICategoryRepository>();
        }

        [Fact]
        public async Task CategoryAdded_WhenValidValuesPassed()
        {
            // Capture
            Category passedCategory = null;
            await repository.AddAsync(Arg.Do<Category>(c => passedCategory = c), Arg.Any<CancellationToken>());

            // Arrange
            var testCategory = new TestData.DefaultCategory();

            // Act
            var command = new CreateCategory.Command(testCategory.Name, testCategory.Note, testCategory.RequireNote);
            await new CreateCategory.Handler(repository).Handle(command, CancellationToken.None);

            // Assert
            await repository.Received().AddAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());
            AssertCategory(passedCategory, testCategory);
        }
    }

}
