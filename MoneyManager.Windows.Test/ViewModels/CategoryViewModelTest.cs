using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.ViewModels.Data;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class CategoryViewModelTest
    {
        private Category category;

        [TestInitialize]
        public async Task InitTests()
        {
            App.CategoryViewModel = new CategoryViewModel();
            App.CategoryViewModel.DeleteAll();

            category = new Category
            {
                Name = "Lohn"
            };
        }

        [TestMethod]
        public void SaveCategoryTest()
        {
            App.CategoryViewModel.Save(category);

            App.CategoryViewModel.LoadList();
            var saved = App.CategoryViewModel.AllCategories.FirstOrDefault(x => x.Id == category.Id);

            Assert.IsTrue(saved.Name == category.Name);
        }

        [TestMethod]
        public void LoadCategoryListTest()
        {
            App.CategoryViewModel.Save(category);
            App.CategoryViewModel.Save(category);
            Assert.AreEqual(App.CategoryViewModel.AllCategories.Count, 2);

            App.CategoryViewModel.AllCategories = null;
            App.CategoryViewModel.LoadList();
            Assert.AreEqual(App.CategoryViewModel.AllCategories.Count, 2);
        }

        [TestMethod]
        public void UpateCategoryTest()
        {
            App.CategoryViewModel.Save(category);
            Assert.AreEqual(App.CategoryViewModel.AllCategories.Count, 1);

            string newName = "This is a new Name";

            App.CategoryViewModel.LoadList();
            category = App.CategoryViewModel.AllCategories.FirstOrDefault();
            category.Name = newName;
            App.CategoryViewModel.Update(category);

            Assert.AreEqual(newName, App.CategoryViewModel.AllCategories.FirstOrDefault().Name);
        }

        [TestMethod]
        public void DeleteCategoryTest()
        {
            App.CategoryViewModel.Save(category);
            Assert.IsTrue(App.CategoryViewModel.AllCategories.Contains(category));

            App.CategoryViewModel.Delete(category);
            Assert.IsFalse(App.CategoryViewModel.AllCategories.Contains(category));
        }
    }
}