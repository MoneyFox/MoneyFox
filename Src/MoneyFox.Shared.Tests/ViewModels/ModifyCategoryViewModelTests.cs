using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Test.Core;
using MoneyFox.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class ModifyCategoryViewModelTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();
        }

        [TestMethod]
        public void Title_EditCategory_CorrectTitle()
        {
            var categoryName = "groceries";

            var settingsManagerMock = new Mock<ISettingsManager>();
            var viewmodel = new ModifyCategoryViewModel(new Mock<ICategoryRepository>().Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = true,
                SelectedCategory = new CategoryViewModel { Id = 9, Name = categoryName }
            };

            viewmodel.Title.ShouldBe(string.Format(Strings.EditCategoryTitle, categoryName));
        }

        [TestMethod]
        public void Title_AddCategory_CorrectTitle()
        {
            var settingsManagerMock = new Mock<ISettingsManager>();
            var viewmodel = new ModifyCategoryViewModel(new Mock<ICategoryRepository>().Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false
            };

            viewmodel.Title.ShouldBe(Strings.AddCategoryTitle); 
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names()
        {
            var categoryList = new List<CategoryViewModel>();

            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            categoryRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<CategoryViewModel, bool>>>()))
                .Returns(categoryList);
            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => { categoryList.Add(cat); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var categoryPrimary = new CategoryViewModel
            {
                Id = 1,
                Name = "Test CategoryViewModel"
            };
            var categorySecondary = new CategoryViewModel
            {
                Name = "Test CategoryViewModel"
            };
            categoryList.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedCategory = categorySecondary
            };

            viewmodel.SaveCommand.Execute();
            categoryList.Count.ShouldBe(1);
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names2()
        {
            var categoryList = new List<CategoryViewModel>();

            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            categoryRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<CategoryViewModel, bool>>>()))
                .Returns(categoryList);
            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => { categoryList.Add(cat); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var categoryPrimary = new CategoryViewModel
            {
                Id = 1,
                Name = "TeSt CATEGory"
            };
            var categorySecondary = new CategoryViewModel
            {
                Name = "Test CategoryViewModel"
            };
            categoryList.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedCategory = categorySecondary
            };

            viewmodel.SaveCommand.Execute();
            categoryList.Count.ShouldBe(1);
        }

        [TestMethod]
        public void SaveCommand_SavesCategory()
        {
            var categoryList = new List<CategoryViewModel>();
            var categoryRepositorySetup = new Mock<ICategoryRepository>();

            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => { categoryList.Add(cat); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var categoryPrimary = new CategoryViewModel
            {
                Id = 1,
                Name = "Test CategoryViewModel",
                Notes = "Test Note"
            };

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedCategory = categoryPrimary
            };

            viewmodel.SaveCommand.Execute();
            categoryList.Count.ShouldBe(1);
        }

        [TestMethod]
        public void SaveCategory_UpdateTimeStamp()
        {
            var category = new CategoryViewModel { Id = 0, Name = "CategoryViewModel", Notes = "" };

            var categoryRepositorySetup = new Mock<ICategoryRepository>();

            categoryRepositorySetup.SetupAllProperties();
            categoryRepositorySetup.Setup(x => x.Save(category)).Returns(true);
            categoryRepositorySetup.Setup(x => x.GetList(null)).Returns(() => new ObservableCollection<CategoryViewModel>());

            var localDateSetting = DateTime.MinValue;

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>()).Callback((DateTime x) => localDateSetting = x);
            
            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
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
            var categoryList = new List<CategoryViewModel>();
            var categoryRepositorySetup = new Mock<ICategoryRepository>();

            categoryRepositorySetup.Setup(c => c.Save(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => { categoryList.Add(cat); });
            categoryRepositorySetup.Setup(c => c.Delete(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => { categoryList.Remove(cat); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var categoryPrimary = new CategoryViewModel
            {
                Id = 1,
                Name = "Test CategoryViewModel",
                Notes = "Notes about the test CategoryViewModel"
            };

            categoryList.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = true,
                SelectedCategory = categoryPrimary
            };

            viewmodel.DeleteCommand.Execute();
            categoryList.Any().ShouldBeFalse();
        }

        [TestMethod]
        public void Cancel_SelectedCategoryReseted()
        {
            string name = "Cateory";
            var baseCategory = new CategoryViewModel { Id = 5, Name = name };
            var category = new CategoryViewModel { Id = 5, Name = name };

            var categoryRepositorySetup = new Mock<ICategoryRepository>();
            categoryRepositorySetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(baseCategory);

            var settingsManagerMock = new Mock<ISettingsManager>();

            var viewmodel = new ModifyCategoryViewModel(categoryRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = true,
                SelectedCategory = category
            };

            viewmodel.SelectedCategory.Name = "foooo";
            viewmodel.CancelCommand.Execute();

            viewmodel.SelectedCategory.Name.ShouldBe(name);
        }

        [TestMethod]
        public void DoneCommand_NameEmpty_ShowMessage()
        {
            // Setup
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string a, string b) => wasDialogServiceCalled = true);

            var settingsManagerMock = new Mock<ISettingsManager>();

            var vm = new ModifyCategoryViewModel(new Mock<ICategoryRepository>().Object,
                dialogSetup.Object,
                settingsManagerMock.Object)
            { SelectedCategory = new CategoryViewModel() };

            // Execute
            vm.SaveCommand.Execute(new CategoryViewModel());

            // Assert
            wasDialogServiceCalled.ShouldBeTrue();
        }
    }
}
