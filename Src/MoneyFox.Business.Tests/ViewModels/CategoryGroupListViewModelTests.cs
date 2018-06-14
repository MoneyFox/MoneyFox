using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoFixture;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Groups;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class CategoryGroupListViewModelTests
    {
        [Fact]
        public void Ctor_ListNotNull()
        {
            // Arrange
            var fixture = new Fixture();
            var vm = fixture.Create<CategoryGroupListViewModel>();

            // Assert
            vm.CategoryGroupList.ShouldNotBeNull();
        }

        [Fact]
        public void IsGroupListEmpty_ListEmpty_True()
        {
            // Arrange
            var fixture = new Fixture();
            var vm = fixture.Create<CategoryGroupListViewModel>();
            vm.CategoryGroupList = new ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>();

            // Assert
            vm.IsGroupListEmpty.ShouldBeTrue();
        }

        [Fact]
        public void IsGroupListEmpty_ListNotEmpty_False()
        {
            // Arrange
            var fixture = new Fixture();
            var vm = fixture.Create<CategoryGroupListViewModel>();
            vm.CategoryGroupList = fixture.Create<ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>>();

            // Assert
            vm.IsGroupListEmpty.ShouldBeFalse();
        }
    }
}
