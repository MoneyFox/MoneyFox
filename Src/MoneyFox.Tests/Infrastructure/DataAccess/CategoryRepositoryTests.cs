namespace MoneyFox.Tests.Infrastructure.DataAccess
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Infrastructure.DataAccess;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using TestFramework.Category;
    using Xunit;
    using static TestFramework.Category.CategoryAssertion;

    public class CategoryRepositoryTests
    {
        private readonly CategoryRepository categoryRepository;
        private readonly AppDbContext appDbContext;

        public CategoryRepositoryTests()
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

        [Fact]
        public async Task AddsCategoryOnlyOnce_WhenAddIsCalledMultipleTimes()
        {
            // Arrange
            var testCategory = new TestData.DefaultCategory();
            var testDbCategory = appDbContext.RegisterCategory(testCategory);

            // Act
            Func<Task> act = async () => await categoryRepository.AddAsync(testDbCategory);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("An item with the same key has already been added. Key: 1");
        }
    }

}
