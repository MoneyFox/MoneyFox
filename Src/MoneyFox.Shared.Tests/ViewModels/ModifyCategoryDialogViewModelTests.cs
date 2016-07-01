using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Test.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyCategoryDialogViewModelTests : MvxIoCSupportingTest {

        [TestInitialize]
        public void Init() {
            Setup();
        }

        [TestMethod]
        public void DoneCommand_NameEmpty_ShowMessage() {
            // Setup
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string a, string b) => wasDialogServiceCalled = true);

            var vm = new ModifyCategoryDialogViewModel(new Mock<IRepository<Category>>().Object,
                dialogSetup.Object);
            vm.Selected = new Category();

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
                new Mock<IDialogService>().Object);
            vm.Selected = categoryToSave;
            
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
            repo.Data = new ObservableCollection<Category>();
            repo.Data.Add(new Category { Name = categoryName });

            var categoryToSave = new Category { Name = "test" };

            var vm = new ModifyCategoryDialogViewModel(repositorySetup.Object, dialogSetup.Object);
            vm.Selected = new Category { Name = categoryName };
            
            // Execute
            vm.DoneCommand.Execute();

            // Assert
            wasDialogServiceCalled.ShouldBeTrue();
        }
    }
}
