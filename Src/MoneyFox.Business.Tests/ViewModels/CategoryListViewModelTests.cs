using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class CategoryListViewModelTests : MvxIoCSupportingTest
    {

        [Fact]
        public void ViewAppearing_PropertiesSet() {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            //var categoryRepoSetup = new Mock<ICategoryService>();
            //categoryRepoSetup.Setup(x => x.GetAllCategories()).ReturnsAsync(() => new List<Category>
            //{
            //    new Category {Data = {Name = string.Empty}}
            //});

            var vm = new CategoryListViewModel(fixture.Create<ICategoryService>(), new Mock<IModifyDialogService>().Object,
                                               new Mock<IDialogService>().Object,
                                               new Mock<IMvxNavigationService>().Object);

            // Act
            vm.ViewAppearing();

            vm.CategoryList.ShouldNotBeNull();
        }
    }
}