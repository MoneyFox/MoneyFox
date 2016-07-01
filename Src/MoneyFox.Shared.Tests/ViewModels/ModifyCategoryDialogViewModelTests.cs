using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyCategoryDialogViewModelTests : MvxIoCSupportingTest {

        [TestInitialize]
        public void Init() {
            Setup();
        }

        [TestMethod]
        public void DoneCommand_NameEmpty_ShowMessage() {
            var wasDialogServiceCalled = false;

            var dialogSetup = new Mock<IDialogService>();
            dialogSetup.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string a, string b) => wasDialogServiceCalled = true);

            var vm = new ModifyCategoryDialogViewModel(new Mock<IRepository<Category>>().Object,
                dialogSetup.Object);
            vm.Selected = new Category();
            vm.DoneCommand.Execute(new Category());

            wasDialogServiceCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void DoneCommand_CategorySaved() {
            Category passedCategory = null;

            var repositorySetup = new Mock<IRepository<Category>>();
            repositorySetup.Setup(x => x.Save(It.IsAny<Category>()))
                .Callback((Category cat) => passedCategory = cat);

            var categoryToSave = new Category { Name = "test" };

            var vm = new ModifyCategoryDialogViewModel(repositorySetup.Object,
                new Mock<IDialogService>().Object);

            vm.Selected = categoryToSave;

            vm.DoneCommand.Execute(categoryToSave);

            passedCategory.ShouldBe(categoryToSave);
        }
    }
}
