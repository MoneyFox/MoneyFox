using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class CategoryViewModelTest
    {
        private Category category;

        private CategoryViewModel categoryViewModel
        {
            get { return new ViewModelLocator().CategoryViewModel; }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            categoryViewModel.DeleteAll();

            category = new Category
            {
                Name = "Lohn"
            };
        }

        [TestMethod]
        public void SaveCategoryTest()
        {
            categoryViewModel.Save(category);

            categoryViewModel.LoadList();
            var saved = categoryViewModel.AllCategories.FirstOrDefault(x => x.Id == category.Id);

            Assert.IsTrue(saved.Name == category.Name);
        }

        [TestMethod]
        public void LoadCategoryListTest()
        {
            categoryViewModel.Save(category);
            categoryViewModel.Save(category);
            Assert.AreEqual(categoryViewModel.AllCategories.Count, 2);

            categoryViewModel.AllCategories = null;
            categoryViewModel.LoadList();
            Assert.AreEqual(categoryViewModel.AllCategories.Count, 2);
        }

        [TestMethod]
        public void UpateCategoryTest()
        {
            categoryViewModel.Save(category);
            Assert.AreEqual(categoryViewModel.AllCategories.Count, 1);

            string newName = "This is a new Name";

            categoryViewModel.LoadList();
            category = categoryViewModel.AllCategories.FirstOrDefault();
            category.Name = newName;
            categoryViewModel.Update(category);

            Assert.AreEqual(newName, categoryViewModel.AllCategories.FirstOrDefault().Name);
        }

        [TestMethod]
        public void DeleteCategoryTest()
        {
            categoryViewModel.Save(category);
            Assert.IsTrue(categoryViewModel.AllCategories.Contains(category));

            categoryViewModel.Delete(category);
            Assert.IsFalse(categoryViewModel.AllCategories.Contains(category));
        }
    }
}