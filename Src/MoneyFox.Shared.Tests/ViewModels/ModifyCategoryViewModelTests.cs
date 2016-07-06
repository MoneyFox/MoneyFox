using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using MoneyFox.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using System.Collections.ObjectModel;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class ModifyCategoryViewModelTests : MvxIoCSupportingTest
    {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        public void Title_EditCategory_CorrectTitle()
        {
            var categoryName = "groceries";

            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            var dialogServiceSetup = new Mock<IDialogService>();

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object)
            {
                IsEdit = true,
                SelectedCategory = new Category { Id = 9, Name = categoryName }
            };

            viewmodel.Title.ShouldBe(string.Format(Strings.EditCategoryTitle, categoryName));
        }

        [TestMethod]
        public void Title_AddCategory_CorrectTitle()
        {
            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            var dialogServiceSetup = new Mock<IDialogService>();

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, dialogServiceSetup.Object)
            {
                IsEdit = false
            };

            viewmodel.Title.ShouldBe(Strings.AddCategoryTitle); 
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names()
        {
            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            var dialogServiceSetup = new Mock<IDialogService>();

            categoryRepositorySetup.SetupAllProperties();
            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<Category>()))
                .Callback((Category cat) => { categoryRepositorySetup.Object.Data.Add(cat); });
            categoryRepositorySetup.Object.Data = new ObservableCollection<Category>();

            var categoryPrimary = new Category
            {
                Id = 1,
                Name = "Test Category"
            };
            var categorySecondary = new Category
            {
                Name = "Test Category"
            };
            categoryRepositorySetup.Object.Data.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, dialogServiceSetup.Object)
            {
                IsEdit = false,
                SelectedCategory = categorySecondary
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, categoryRepositorySetup.Object.Data.Count);
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names2()
        {
            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            var dialogServiceSetup = new Mock<IDialogService>();

            categoryRepositorySetup.SetupAllProperties();
            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<Category>()))
                .Callback((Category cat) => { categoryRepositorySetup.Object.Data.Add(cat); });
            categoryRepositorySetup.Object.Data = new ObservableCollection<Category>();

            var categoryPrimary = new Category
            {
                Id = 1,
                Name = "TeSt CATEGory"
            };
            var categorySecondary = new Category
            {
                Name = "Test Category"
            };
            categoryRepositorySetup.Object.Data.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, dialogServiceSetup.Object)
            {
                IsEdit = false,
                SelectedCategory = categorySecondary
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, categoryRepositorySetup.Object.Data.Count);
        }

        [TestMethod]
        public void SaveCommand_SavesCategory()
        {
            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            var dialogServiceSetup = new Mock<IDialogService>();

            categoryRepositorySetup.SetupAllProperties();
            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<Category>()))
                .Callback((Category cat) => { categoryRepositorySetup.Object.Data.Add(cat); });
            categoryRepositorySetup.Object.Data = new ObservableCollection<Category>();

            var categoryPrimary = new Category
            {
                Id = 1,
                Name = "Test Category",
                Notes = "Test Note"
            };

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, dialogServiceSetup.Object)
            {
                IsEdit = false,
                SelectedCategory = categoryPrimary
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, categoryRepositorySetup.Object.Data.Count);
        }

        [TestMethod]
        public void SaveCategory_UpdateTimeStamp()
        {
            var category = new Category { Id = 0, Name = "category", Notes = "" };

            var categoryRepositorySetup = new Mock<ICategoryRepository>();

            categoryRepositorySetup.SetupAllProperties();
            categoryRepositorySetup.Setup(x => x.Save(category)).Returns(true);
            categoryRepositorySetup.Setup(x => x.Data).Returns(() => new ObservableCollection<Category>());
            var categoryRepo = categoryRepositorySetup.Object;

            var viewmodel = new ModifyCategoryViewModel(categoryRepo, new Mock<IDialogService>().Object)
            {
                IsEdit = false,
                SelectedCategory = category
            };

            viewmodel.SaveCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

        [TestMethod]
        public void DeleteCategory_DeletesCategory()
        {
            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            var dialogServiceSetup = new Mock<IDialogService>();

            categoryRepositorySetup.SetupAllProperties();
            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<Category>()))
                .Callback((Category cat) => { categoryRepositorySetup.Object.Data.Add(cat); });
            categoryRepositorySetup.Setup(c => c.Delete(It.IsAny<Category>()))
                .Callback((Category cat) => { categoryRepositorySetup.Object.Data.Remove(cat); });
            categoryRepositorySetup.Object.Data = new ObservableCollection<Category>();

            var categoryPrimary = new Category
            {
                Id = 1,
                Name = "Test Category",
                Notes = "Notes about the test category"
            };

            categoryRepositorySetup.Object.Data.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, dialogServiceSetup.Object)
            {
                IsEdit = true,
                SelectedCategory = categoryPrimary
            };

            viewmodel.DeleteCommand.Execute();
            Assert.AreEqual(0, categoryRepositorySetup.Object.Data.Count);
        }
    }
}
