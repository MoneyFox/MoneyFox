using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyCategoryDialogViewModelTests : MvxIoCSupportingTest {

        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        public void DoneCommand_NameEmpty_ShowMessage() {
            // Setup
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string a, string b) => wasDialogServiceCalled = true);

            var vm = new ModifyCategoryDialogViewModel(new Mock<IRepository<Category>>().Object,
                dialogSetup.Object) {Selected = new Category()};

            // Execute
            vm.DoneCommand.Execute(new Category());

            // Assert
            wasDialogServiceCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void DoneCommand_CategorySaved() {
            // Setup
            Category passedCategory = null;

            var repositorySetup = new Mock<IRepository<Category>>();
            repositorySetup.Setup(x => x.Save(It.IsAny<Category>()))
                .Callback((Category cat) => passedCategory = cat);
            repositorySetup.SetupGet(x => x.Data).
                Returns(new ObservableCollection<Category>());

            var categoryToSave = new Category { Name = "test" };

            var vm = new ModifyCategoryDialogViewModel(repositorySetup.Object,
                new Mock<IDialogService>().Object) {Selected = categoryToSave};

            // Execute
            vm.DoneCommand.Execute();

            // Assert
            passedCategory.ShouldBe(categoryToSave);
        }

        [TestMethod]
        public void DoneCommand_NameAlreadyTaken_ShowMessage() {
            // Setup
            const string categoryName = "Test name Category";
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string a, string b) => wasDialogServiceCalled = true);

            var repositorySetup = new Mock<IRepository<Category>>();
            repositorySetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()));
            repositorySetup.SetupAllProperties();

            var repo = repositorySetup.Object;
            repo.Data = new ObservableCollection<Category> {new Category {Name = categoryName}};

            var vm = new ModifyCategoryDialogViewModel(repositorySetup.Object, dialogSetup.Object) {
                Selected = new Category {Name = categoryName}
            };

            // Execute
            vm.DoneCommand.Execute();

            // Assert
            wasDialogServiceCalled.ShouldBeTrue();
        }


        [TestMethod]
        public void DoneCommand_NameAlreadyTakenToUpper_ShowMessage() {
            // Setup
            const string categoryName1 = "Test name Category";
            const string categoryName2 = "Test name CATegory";
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string a, string b) => wasDialogServiceCalled = true);

            var repositorySetup = new Mock<IRepository<Category>>();
            repositorySetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()));
            repositorySetup.SetupAllProperties();

            var repo = repositorySetup.Object;
            repo.Data = new ObservableCollection<Category> { new Category { Name = categoryName1 } };

            var vm = new ModifyCategoryDialogViewModel(repositorySetup.Object, dialogSetup.Object) {
                Selected = new Category { Name = categoryName2 }
            };

            // Execute
            vm.DoneCommand.Execute();

            // Assert
            wasDialogServiceCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            Category category = new Category { Id = 1, Name = "categpry2" };
            Mock<IRepository<Category>> categoryRepoMock = new Mock<IRepository<Category>>();
            categoryRepoMock.SetupAllProperties();
            ObservableCollection<Category> categories = new ObservableCollection<Category> {
                new Category {Id = 0, Name = "category"}
            };
            categoryRepoMock.Setup(x => x.Data).Returns(categories);
            categoryRepoMock.Setup(x => x.Save(category)).Returns(true);

            var vm = new ModifyCategoryDialogViewModel(categoryRepoMock.Object,
                new Mock<IDialogService>().Object) {Selected = category};
            vm.DoneCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }
    }
}
