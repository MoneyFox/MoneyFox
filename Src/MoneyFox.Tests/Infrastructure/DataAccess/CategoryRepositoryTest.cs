namespace MoneyFox.Tests.Infrastructure.DataAccess
{

    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using MoneyFox.Core.Aggregates.CategoryAggregate;
    using MoneyFox.Infrastructure.DataAccess;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    public class CategoryRepositoryTest
    {
        private readonly CategoryRepository categoryRepository;
        private readonly AppDbContext appDbContext;

        public CategoryRepositoryTest()
        {
            appDbContext = InMemoryAppDbContextFactory.Create();
            categoryRepository = new CategoryRepository(appDbContext);
        }

        [Fact]
        public async Task CategoryAdded_WhenNewlyCreated()
        {
            // Arrange
            var testCategory = new TestData.DefaultCategory();

            // Act
            await categoryRepository.AddAsync(testCategory.CreateDbCategory());

            // Assert
            appDbContext.Categories.Should().ContainSingle();
            var loadedCategory = appDbContext.Categories.Single();
            AssertCategory(actual: loadedCategory, expected: testCategory);
        }

        private static void AssertCategory(Category actual, TestData.DefaultCategory expected)
        {
            using (new AssertionScope())
            {
                actual.Name.Should().Be(expected.Name);
                actual.Note.Should().Be(expected.Note);
                actual.RequireNote.Should().Be(expected.RequireNote);
            }
        }
    }

}
