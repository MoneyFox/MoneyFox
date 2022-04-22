namespace MoneyFox.Infrastructure.Tests.DataAccess
{

    using Infrastructure.DataAccess;
    using Xunit;

    public class CategoryRepositoryTest
    {

        private readonly CategoryRepository categoryRepository;

        public CategoryRepositoryTest()
        {
            var context = InMemoryAppDbContextFactory.Create();
            categoryRepository = new CategoryRepository();
        }

        [Fact]
        public void Foo()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
