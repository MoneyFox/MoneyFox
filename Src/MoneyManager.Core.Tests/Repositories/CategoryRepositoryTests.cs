using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Repositories
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private CategoryDataAccessMock _categoryDataAccessMock;

        [TestInitialize]
        public void Init()
        {
            _categoryDataAccessMock = new CategoryDataAccessMock();
        }

        [TestMethod]
        public void CategoryRepository_Save()
        {
            var repository = new CategoryRepository(_categoryDataAccessMock);

            var category = new Category
            {
                Name = "Ausgang"
            };

            repository.Save(category);

            Assert.IsTrue(category == _categoryDataAccessMock.CategoryTestList[0]);
        }

        [TestMethod]
        public void CategoryRepository_SaveWithouthName()
        {
            var repository = new CategoryRepository(_categoryDataAccessMock);
            var category = new Category();

            repository.Save(category);

            Assert.AreSame(category, repository.Data[0]);
            Assert.IsTrue(Strings.NoNamePlaceholderLabel == repository.Data[0].Name);
        }

        [TestMethod]
        public void CategoryRepository_Delete()
        {
            var repository = new CategoryRepository(_categoryDataAccessMock);

            var category = new Category
            {
                Name = "Ausgang"
            };

            repository.Save(category);
            Assert.AreSame(category, _categoryDataAccessMock.CategoryTestList[0]);

            repository.Delete(category);

            Assert.IsFalse(_categoryDataAccessMock.CategoryTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void CategoryRepository_AccessCache()
        {
            Assert.IsNotNull(new CategoryRepository(_categoryDataAccessMock).Data);
        }

        [TestMethod]
        public void CategoryRepository_AddMultipleToCache()
        {
            var repository = new CategoryRepository(_categoryDataAccessMock);
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

            Assert.AreEqual(2, repository.Data.Count);
            Assert.AreSame(category, repository.Data[0]);
            Assert.AreSame(secondCategory, repository.Data[1]);
        }
    }
}