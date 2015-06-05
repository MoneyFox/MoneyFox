using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.WindowsPhone.Test.Mocks;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories
{
    [TestClass]
    public class CategoryRepositoryTest
    {
        private CategoryDataAccessMock _categoryDataAccessMock;

        [TestInitialize]
        public void Init()
        {
            _categoryDataAccessMock = new CategoryDataAccessMock();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void CategoryRepositor_LoadDataFromDbThroughRepository()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.DeleteAll<Category>();
                db.InsertWithChildren(new Category
                {
                    Name = "Foooo"
                });
            }

            var repository = new CategoryRepository(new CategoryDataAccess());

            Assert.IsTrue(repository.Data.Any());
            Assert.AreEqual("Foooo", repository.Data[0].Name);
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
            Assert.IsTrue(Translation.GetTranslation("NoNamePlaceholderLabel") == repository.Data[0].Name);
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

        [TestMethod]
        [TestCategory("Integration")]
        public void CategoryRepository_Update()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.DeleteAll<Category>();
            }

            var repository = new CategoryRepository(new CategoryDataAccess());

            var category = new Category
            {
                Name = "Ausgang"
            };

            repository.Save(category);
            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreSame(category, repository.Data[0]);

            category.Name = "newName";

            repository.Save(category);

            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreEqual("newName", repository.Data[0].Name);
        }
    }
}