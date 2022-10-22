namespace MoneyFox.Infrastructure.Tests.DataAccess
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Tests.TestFramework;
    using FluentAssertions;
    using MoneyFox.Infrastructure.DataAccess;
    using MoneyFox.Infrastructure.Persistence;
    using Xunit;
    using static Core.Tests.TestFramework.CategoryAssertion;

    public class CategoryRepositoryTests
    {
        private readonly AppDbContext appDbContext;
        private readonly CategoryRepository categoryRepository;

        protected CategoryRepositoryTests()
        {
            appDbContext = InMemoryAppDbContextFactory.Create();
            categoryRepository = new CategoryRepository(appDbContext);
        }

        public sealed class AddAsync : CategoryRepositoryTests
        {
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

}
