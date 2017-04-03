using System.Collections.Generic;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
using Moq;
using MvvmCross.Test.Core;
using Xunit;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class CategoryListViewModelTests : MvxIoCSupportingTest
    {

        [Fact]
        public void Loaded_PropertiesSet()
        {
            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(null)).Returns(() => new List<CategoryViewModel>
            {
                new CategoryViewModel {Name = string.Empty}
            });

            var vm = new CategoryListViewModel(categoryRepoSetup.Object, new Mock<IModifyDialogService>().Object, new Mock<IDialogService>().Object);
            vm.LoadedCommand.Execute();

            vm.Source.ShouldNotBeNull();
        }
    }
}