using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class CategoryListViewModelTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [TestMethod]
        public void Ctor_Default()
        {
            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupGet(x => x.Data).Returns(() => new ObservableCollection<Category>
            {
                new Category {Name = string.Empty}
            });

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.CategoryRepository).Returns(categoryRepoSetup.Object);

            var vm = new CategoryListViewModel(unitOfWork.Object, new Mock<IDialogService>().Object);
            vm.Source.ShouldNotBeNull();
        }
    }
}