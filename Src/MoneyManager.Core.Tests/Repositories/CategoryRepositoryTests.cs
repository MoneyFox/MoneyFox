using System.Globalization;
using System.Linq;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using Xunit;

namespace MoneyManager.Core.Tests.Repositories
{
    public class CategoryRepositoryTests
    {
        [Theory]
        [InlineData("Ausgang", "Ausgang")]
        [InlineData("", "[No Name]")]
        public void Save_InputName_CorrectNameAssigned(string inputName, string expectedResult)
        {
            Strings.Culture = new CultureInfo("en-US");
            var categoryDataAccessMock = new CategoryDataAccessMock();
            var repository = new CategoryRepository(categoryDataAccessMock);

            var category = new Category
            {
                Name = inputName
            };

            repository.Save(category);

            categoryDataAccessMock.CategoryTestList[0].ShouldBeSameAs(category);
            categoryDataAccessMock.CategoryTestList[0].Name.ShouldBe(expectedResult);
        }

        [Fact]
        public void CategoryRepository_Delete()
        {
            var categoryDataAccessMock = new CategoryDataAccessMock();
            var repository = new CategoryRepository(categoryDataAccessMock);

            var category = new Category
            {
                Name = "Ausgang"
            };

            repository.Save(category);

            categoryDataAccessMock.CategoryTestList[0].ShouldBeSameAs(category);

            repository.Delete(category);

            categoryDataAccessMock.CategoryTestList.Any().ShouldBeFalse();
            repository.Data.Any().ShouldBeFalse();
        }

        [Fact]
        public void CategoryRepository_AccessCache()
        {
            new CategoryRepository(new CategoryDataAccessMock()).Data.ShouldNotBeNull();
        }

        [Fact]
        public void CategoryRepository_AddMultipleToCache()
        {
            var repository = new CategoryRepository(new CategoryDataAccessMock());
            var category = new Category
            {
                Name = "Ausgang"
            };

            var secondCategory = new Category
            {
                Name = "Lebensmittel"
            };

            repository.Save(category);
            repository.Save(secondCategory);

            repository.Data.Count.ShouldBe(2);
            repository.Data[0].ShouldBeSameAs(category);
            repository.Data[1].ShouldBeSameAs(secondCategory);
        }
    }
}