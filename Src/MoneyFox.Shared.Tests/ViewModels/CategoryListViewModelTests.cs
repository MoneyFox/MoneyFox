using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
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
            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(null)).Returns(() => new List<CategoryViewModel>
            {
                new CategoryViewModel {Name = string.Empty}
            });

            var vm = new CategoryListViewModel(categoryRepoSetup.Object, new Mock<IDialogService>().Object);
            vm.Source.ShouldNotBeNull();
        }
    }
}