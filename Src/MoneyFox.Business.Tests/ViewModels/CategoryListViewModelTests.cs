using System.Collections.Generic;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Test.Core;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class CategoryListViewModelTests : MvxIoCSupportingTest
    {

        [Fact]
        public void Loaded_PropertiesSet()
        {
            var categoryRepoSetup = new Mock<ICategoryService>();
            categoryRepoSetup.Setup(x => x.GetAllCategories()).ReturnsAsync(() => new List<Category>
            {
                new Category {Data = {Name = string.Empty}}
            });

            var vm = new CategoryListViewModel(categoryRepoSetup.Object, new Mock<IModifyDialogService>().Object,
                                               new Mock<IDialogService>().Object,
                                               new Mock<IMvxNavigationService>().Object);
            vm.LoadedCommand.Execute();

            vm.CategoryList.ShouldNotBeNull();
        }
    }
}