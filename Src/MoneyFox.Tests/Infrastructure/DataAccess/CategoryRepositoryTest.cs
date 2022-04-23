namespace MoneyFox.Tests.Infrastructure.DataAccess
{

    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Infrastructure.DataAccess;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using TestFramework.Category;
    using Xunit;
    using static TestFramework.Category.CategoryAssertion;

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
    }

}
