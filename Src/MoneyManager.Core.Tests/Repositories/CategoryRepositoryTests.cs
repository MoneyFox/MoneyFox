using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Localization;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Repositories
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        public static IEnumerable NamePlaceholder
        {
            get
            {
                yield return new object[] {"Ausgang", "Ausgang"};
                yield return new object[] {"", Strings.NoNamePlaceholderLabel};
            }
        }


        [Theory]
        [MemberData(nameof(NamePlaceholder))]
        public void Save_InputName_CorrectNameAssigned(string inputName, string expectedResult)
        {
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

        [TestMethod]
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

        [TestMethod]
        public void CategoryRepository_AccessCache()
        {
            new CategoryRepository(new CategoryDataAccessMock()).Data.ShouldNotBeNull();
        }

        [TestMethod]
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

        [TestMethod]
        public void Load_CategoryDataAccess_DataInitialized()
        {
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>
            {
                new Category {Id = 10},
                new Category {Id = 15}
            });

            var categoryRepository = new CategoryRepository(dataAccessSetup.Object);
            categoryRepository.Load();

            categoryRepository.Data.Any(x => x.Id == 10).ShouldBeTrue();
            categoryRepository.Data.Any(x => x.Id == 15).ShouldBeTrue();
        }
    }
}