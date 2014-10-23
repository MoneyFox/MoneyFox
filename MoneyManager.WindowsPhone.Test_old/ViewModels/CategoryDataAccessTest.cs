using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using System.Linq;

namespace MoneyManager.WindowsPhone.Test.ViewModels
{
    [TestClass]
    internal class CategoryDataAccessTest
    {
        private Category category;

        private CategoryDataAccess CategoryDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        [TestInitialize]
        public void InitTests()
        {
            CategoryDataAccess.DeleteAll();

            category = new Category
            {
                Name = "Lohn"
            };
        }

        [TestMethod]
        public void SaveCategoryTest()
        {
            CategoryDataAccess.Save(category);

            CategoryDataAccess.LoadList();
            Category saved = CategoryDataAccess.AllCategories.FirstOrDefault(x => x.Id == category.Id);

            Assert.IsTrue(saved.Name == category.Name);
        }

        [TestMethod]
        public void LoadCategoryListTest()
        {
            CategoryDataAccess.Save(category);
            CategoryDataAccess.Save(category);
            Assert.AreEqual(CategoryDataAccess.AllCategories.Count, 2);

            CategoryDataAccess.AllCategories = null;
            CategoryDataAccess.LoadList();
            Assert.AreEqual(CategoryDataAccess.AllCategories.Count, 2);
        }

        [TestMethod]
        public void UpateCategoryTest()
        {
            CategoryDataAccess.Save(category);
            Assert.AreEqual(CategoryDataAccess.AllCategories.Count, 1);

            string newName = "This is a new Name";

            CategoryDataAccess.LoadList();
            category = CategoryDataAccess.AllCategories.FirstOrDefault();
            category.Name = newName;
            CategoryDataAccess.Update(category);

            Assert.AreEqual(newName, CategoryDataAccess.AllCategories.FirstOrDefault().Name);
        }

        [TestMethod]
        public void DeleteCategoryTest()
        {
            CategoryDataAccess.Save(category);
            Assert.IsTrue(CategoryDataAccess.AllCategories.Contains(category));

            CategoryDataAccess.Delete(category, true);
            Assert.IsFalse(CategoryDataAccess.AllCategories.Contains(category));
        }
    }
}