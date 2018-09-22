using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class ModifyCategoryViewModelTests
    {
        [Fact]
        public void Cancel_SelectedCategoryReseted()
        {
            // Arrange
            string name = "Cateory";
            var baseCategory = new Category {Data = {Id = 5, Name = name}};
            var category = new Category {Data = {Id = 5, Name = name}};

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(baseCategory);

            var settingsManagerMock = new Mock<ISettingsManager>();

            var viewmodel = new ModifyCategoryViewModel(categoryServiceMock.Object,
                                                        new Mock<IDialogService>().Object,
                                                        settingsManagerMock.Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = true,
                SelectedCategory = new CategoryViewModel(category)
            };

            // Act
            viewmodel.SelectedCategory.Name = "foooo";
            viewmodel.CancelCommand.Execute();

            // Assert
            viewmodel.SelectedCategory.Name.ShouldEqual(name);
        }

        [Fact]
        public void DeleteCategory_DeletesCategory()
        {
            // Arrange
            var categoryList = new List<Category>();
            var categoryServiceMock = new Mock<ICategoryService>();

            categoryServiceMock.Setup(c => c.SaveCategory(It.IsAny<Category>()))
                               .Callback((Category cat) => { categoryList.Add(cat); });
            categoryServiceMock.Setup(c => c.DeleteCategory(It.IsAny<Category>()))
                               .Callback((Category cat) => { categoryList.Remove(cat); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var categoryPrimary = new Category
            {
                Data =
                {
                    Id = 1,
                    Name = "Test CategoryViewModel",
                    Note = "Notes about the test CategoryViewModel"
                }
            };

            categoryList.Add(categoryPrimary);

            var viewmodel = new ModifyCategoryViewModel(categoryServiceMock.Object,
                                                        new Mock<IDialogService>().Object,
                                                        settingsManagerMock.Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = true,
                SelectedCategory = new CategoryViewModel(categoryPrimary)
            };

            // Act
            viewmodel.DeleteCommand.Execute();

            // Assert
            categoryList.Any().ShouldBeFalse();
        }

        [Fact]
        public void DoneCommand_NameEmpty_ShowMessage()
        {
            // Arrange
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                       .Callback((string a, string b) => wasDialogServiceCalled = true)
                       .Returns(Task.FromResult(0));

            var settingsManagerMock = new Mock<ISettingsManager>();

            var vm = new ModifyCategoryViewModel(new Mock<ICategoryService>().Object,
                                                 dialogSetup.Object,
                                                 settingsManagerMock.Object,
                                                 new Mock<IBackupManager>().Object,
                                                 new Mock<IMvxLogProvider>().Object,
                                                 new Mock<IMvxNavigationService>().Object)
                {SelectedCategory = new CategoryViewModel(new Category())};

            // Act
            vm.SaveCommand.Execute(new CategoryViewModel(new Category()));

            // Assert
            wasDialogServiceCalled.ShouldBeTrue();
        }

        [Fact]
        public void SaveCategory_UpdateTimeStamp()
        {
            // Arrange
            var category = new CategoryViewModel(new Category()) {Id = 0, Name = "CategoryViewModel", Notes = ""};

            var categoryServiceMock = new Mock<ICategoryService>();

            categoryServiceMock.SetupAllProperties();
            categoryServiceMock.Setup(x => x.SaveCategory(It.IsAny<Category>())).Returns(Task.CompletedTask);
            categoryServiceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(new ObservableCollection<Category>());

            var localDateSetting = DateTime.MinValue;

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>())
                               .Callback((DateTime x) => localDateSetting = x);

            var viewmodel = new ModifyCategoryViewModel(categoryServiceMock.Object,
                                                        new Mock<IDialogService>().Object,
                                                        settingsManagerMock.Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = false,
                SelectedCategory = category
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

        [Fact]
        public void SaveCommand_DoesNotAllowDuplicateNames()
        {
            // Arrange
            var saveWasCalled = false;
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(c => c.CheckIfNameAlreadyTaken(It.IsAny<string>()))
                               .ReturnsAsync(true);
            categoryServiceMock.Setup(c => c.SaveCategory(It.IsAny<Category>()))
                               .Callback(() => saveWasCalled = true)
                               .Returns(Task.CompletedTask);

            var categoryPrimary = new Category
            {
                Data =
                {
                    Id = 1,
                    Name = "Test CategoryViewModel"
                }
            };

            var viewmodel = new ModifyCategoryViewModel(categoryServiceMock.Object,
                                                        new Mock<IDialogService>().Object,
                                                        new Mock<ISettingsManager>().Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = false,
                SelectedCategory = new CategoryViewModel(categoryPrimary)
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            saveWasCalled.ShouldBeFalse();
        }

        [Fact]
        public void SaveCommand_SavesCategory()
        {
            // Arrange
            var categoryList = new List<Category>();
            var categoryServiceMock = new Mock<ICategoryService>();

            categoryServiceMock.Setup(c => c.SaveCategory(It.IsAny<Category>()))
                               .Callback((Category cat) => { categoryList.Add(cat); })
                               .Returns(Task.CompletedTask);

            var settingsManagerMock = new Mock<ISettingsManager>();

            var categoryPrimary = new Category
            {
                Data =
                {
                    Id = 1,
                    Name = "Test CategoryViewModel",
                    Note = "Test Note"
                }
            };

            var viewmodel = new ModifyCategoryViewModel(categoryServiceMock.Object,
                                                        new Mock<IDialogService>().Object,
                                                        settingsManagerMock.Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = false,
                SelectedCategory = new CategoryViewModel(categoryPrimary)
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            categoryList.Count.ShouldEqual(1);
        }

        [Fact]
        public void Title_AddCategory_CorrectTitle()
        {
            // Arrange
            var viewmodel = new ModifyCategoryViewModel(new Mock<ICategoryService>().Object,
                                                        new Mock<IDialogService>().Object,
                                                        new Mock<ISettingsManager>().Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = false
            };

            // Act / Assert
            viewmodel.Title.ShouldEqual(Strings.AddCategoryTitle);
        }

        [Fact]
        public void Title_EditCategory_CorrectTitle()
        {
            // Arrange
            var categoryName = "groceries";

            var viewmodel = new ModifyCategoryViewModel(new Mock<ICategoryService>().Object,
                                                        new Mock<IDialogService>().Object,
                                                        new Mock<ISettingsManager>().Object,
                                                        new Mock<IBackupManager>().Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = true,
                SelectedCategory = new CategoryViewModel(new Category()) {Id = 9, Name = categoryName}
            };

            // Act / Assert
            viewmodel.Title.ShouldEqual(string.Format(Strings.EditCategoryTitle, categoryName));
        }
    }
}